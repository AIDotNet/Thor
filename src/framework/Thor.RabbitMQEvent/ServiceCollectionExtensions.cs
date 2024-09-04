using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Raccoon.Stack.Rabbit;
using Thor.BuildingBlocks.Data;

namespace Thor.RabbitMQEvent;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        // 是否启用RabbitMQ
        var connection = configuration["RabbitMQ:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connection))
        {
            return services;
        }

        services
            .AddSingleton(typeof(IEventBus<>), typeof(RabbitMQEventBus<>));

        services.AddRabbitBoot((options =>
        {
            options.ConnectionString = connection;
            options.Consumes =
            [
                new ConsumeOptions
                {
                    AutoAck = false,
                    FetchCount = 10,
                    Queue = "Thor:EventBus",
                    Declaration = (declaration =>
                    {
                        declaration.QueueDeclareAsync("Thor:EventBus", true);
                        declaration.ExchangeDeclareAsync("Thor:EventBus:exchange", ExchangeType.Direct, true);
                        declaration.QueueBindAsync("Thor:EventBus", "Thor:EventBus:exchange", "Thor:EventBus:key");
                    })
                }
            ];
        }), typeof(RabbitMQEventBus<>).Assembly);

        return services;
    }
}