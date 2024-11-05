namespace Thor.Rabbit;

public abstract class RabbitClient(
    ILogger<RabbitClient> logger,
    RabbitOptions rabbitOptions,
    IServiceProvider serviceProvider)
{
    private readonly ILogger _logger = logger;

    protected ConcurrentDictionary<IConnection, int> Connections = new();
    protected ConcurrentDictionary<string, Lazy<Task<IChannel>>> Channels = new();

    protected readonly ConcurrentDictionary<string,
            Channel<(object model, BasicDeliverEventArgs args, ConsumeOptions options)>>
        Queue = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var cf = new ConnectionFactory
        {
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(30),
            Uri = new Uri(rabbitOptions.ConnectionString)
        };

        Connections.TryAdd(await cf.CreateConnectionAsync(cancellationToken).ConfigureAwait(false), 0);

        if ((rabbitOptions.Consumes?.Count ?? 0) > 0)
        {
            foreach (var opt in rabbitOptions.Consumes)
            {
                await SubscribeAsync(opt).ConfigureAwait(false);
            }
        }
    }

    public async Task SubscribeAsync(ConsumeOptions consumeOptions)
    {
        // 通过GetOrAdd确保同一个队列只有一个channel
        _ = await Channels.GetOrAdd(consumeOptions.Queue,
                queue => new Lazy<Task<IChannel>>(() => CreateAndConfigureChannel(queue, consumeOptions))).Value
            .ConfigureAwait(false);
    }

    private async Task<IChannel> CreateAndConfigureChannel(string queue, ConsumeOptions consumeOptions)
    {
        var opt = consumeOptions;
        var channel = await GetConnection().CreateChannelAsync();
        var decl = new DefaultDeclaration(channel);
        opt.Declaration.Invoke(decl);

        if (opt.FetchCount > 0)
        {
            await channel.BasicQosAsync(0, opt.FetchCount, false);
        }

        var consumer = new AsyncEventingBasicConsumer(channel);

        if (opt.FetchCount > 1)
        {
            var asyncServiceScopes = new List<AsyncServiceScope>();
            for (int i = 0; i < opt.FetchCount; i++)
            {
                asyncServiceScopes.Add(serviceProvider.CreateAsyncScope());
            }

            // 将设计一个Handler一个Scope，然后通过ConcurrentBag来管理
            var scopeBag = new ConcurrentBag<AsyncServiceScope>(asyncServiceScopes);

            // 根据FetchCount创建固定的Task数量，然后Received事件中通过Task.Run执行
            consumer.Received += async (obj, arg) =>
            {
                _ = Task.Run(async () =>
                {
                    if (scopeBag.TryTake(out var scope))
                    {
                        try
                        {
                            var bus = scope.ServiceProvider.GetRequiredService<IRabbitEventBus>();
                            await bus.Trigger(scope.ServiceProvider, arg, opt);
                            await channel.BasicAckAsync(arg.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"rabbit on queue({opt.Queue}) received error: {ex}");
                            await channel.BasicNackAsync(arg.DeliveryTag, false, opt.FailedRequeue);
                        }
                        finally
                        {
                            scopeBag.Add(scope);
                        }
                    }
                });

                await Task.CompletedTask.ConfigureAwait(false);
            };
        }
        else
        {
            var asyncServiceScope = serviceProvider.CreateAsyncScope();
            consumer.Received += async (obj, arg) =>
            {
                try
                {
                    var bus = asyncServiceScope.ServiceProvider.GetRequiredService<IRabbitEventBus>();
                    await bus.Trigger(asyncServiceScope.ServiceProvider, arg, opt);
                    await channel.BasicAckAsync(arg.DeliveryTag, false);
                    await channel.BasicAckAsync(arg.DeliveryTag, false).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"rabbit on queue({opt.Queue}) received error: {ex}");
                    await channel.BasicNackAsync(arg.DeliveryTag, false, opt.FailedRequeue).ConfigureAwait(false);
                }
            };
        }


        // 当通道调用的回调中发生异常时发出信号
        channel.CallbackException += (_, args) => { _logger.LogError(args.Exception, args.Exception.Message); };
        await channel.BasicConsumeAsync(queue, opt.AutoAck, consumer).ConfigureAwait(false);

        return channel;
    }


    public async Task UnSubscribe(string queue, string exchange, string routingKey, bool deleteQueue = false)
    {
        if (!Channels.TryGetValue(queue, out var valueTask))
        {
            return;
        }

        var channel = await valueTask.Value;

        _logger.LogInformation($"尝试解除rabbit绑定 queue:{queue}, exchange:{exchange}, routingKey:{routingKey}");
        await channel.QueueUnbindAsync(queue, exchange, routingKey);

        if (deleteQueue)
        {
            var passive = await channel.QueueDeclarePassiveAsync(queue).ConfigureAwait(false);
            _logger.LogInformation(
                $"尝试删除rabbit queue:{passive.QueueName} consumerCount:{passive.ConsumerCount} messageCount:{passive.MessageCount}");
            await channel.QueueDeleteAsync(queue).ConfigureAwait(false);
        }

        await channel.CloseAsync();
        Channels.TryRemove(queue, out _);
        Queue.TryRemove(queue, out var item);

        // 取消item中的所有等待
        item?.Writer.TryComplete();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Channels != null)
        {
            foreach (var kv in Channels)
            {
                await (await kv.Value.Value.ConfigureAwait(false)).CloseAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            Channels = null;
        }

        if (Connections != null)
        {
            foreach (var kv in Connections)
            {
                await kv.Key.CloseAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            Connections = null;
        }
    }

    public async Task PublishAsync(string exchange, string routingKey, byte[] data,
        Action<IReadOnlyBasicProperties> options = null)
    {
        using var activity = RabbitInstrumentation.ActivitySource.StartActivity($"/{exchange}/{routingKey}");
        activity?.SetTag("kind", "publish");

        var key = $"{exchange}:{routingKey}";
        var channel = await Channels.GetOrAdd(key, k => new Lazy<Task<IChannel>>(GetConnection().CreateChannelAsync()))
            .Value.ConfigureAwait(false);
        var prop = new BasicProperties() { Headers = new Dictionary<string, object>() };
        // TProperties
        options?.Invoke(prop);

        await channel.BasicPublishAsync(exchange: exchange, routingKey: routingKey, body: data, basicProperties: prop);
    }

    private IConnection GetConnection()
    {
        var con = Connections.OrderBy(q => q.Value).Select(q => q.Key).FirstOrDefault();
        Connections.AddOrUpdate(con, k => 1, (k, v) => v + 1);
        return con;
    }
}