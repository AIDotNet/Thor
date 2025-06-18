using MapsterMapper;
using MiniExcelLibs;
using OpenAI.Chat;
using System.Diagnostics;
using System.Threading.Channels;
using MiniExcelLibs.Attributes;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Consts;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.AzureOpenAI;
using Thor.Claude;
using Thor.Core;
using Thor.Hunyuan;
using Thor.DeepSeek;
using Thor.Domain.Chats;
using Thor.OpenAI;
using Thor.Service.Infrastructure;
using Thor.SparkDesk;

namespace Thor.Service.Service;

/// <summary>
/// 渠道管理
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="mapper"></param>
public sealed class ChannelService(
    IServiceProvider serviceProvider,
    IMapper mapper,
    IServiceCache cache)
    : ApplicationService(serviceProvider)
{
    private const string CacheKey = "CacheKey:Channel";

    /// <summary>
    /// 获取渠道列表 如果缓存中有则从缓存中获取
    /// </summary>
    public async Task<ChatChannel[]> GetChannelsAsync()
    {
        return await cache.GetOrCreateAsync(CacheKey,
            async () => { return await DbContext.Channels.AsNoTracking().Where(x => !x.Disable).ToArrayAsync(); },
            isLock: false);
    }

    /// <summary>
    /// 获取包含指定模型名的渠道列表 如果缓存中有则从缓存中获取
    /// </summary>
    /// <param name="model">模型名</param>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async ValueTask<IEnumerable<ChatChannel>> GetChannelsContainsModelAsync(string model, User user,
        Token? token, bool isResponses = false)
    {
        var group = token?.Groups ?? (user).Groups;
        return (await GetChannelsAsync()).Where(x =>
            x.Models.Contains(model)
            && x.SupportsResponses == isResponses // 是否支持Responses
            // 防止重试重复分配
            && !ChannelAsyncLocal.ChannelIds.Contains(x.Id) &&
            (group.Length == 0 || x.Groups.Select(x => x.ToLower()).Intersect(group).Any()));
    }

    /// <summary>
    /// 创建渠道
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    public async ValueTask CreateAsync(ChatChannelInput channel)
    {
        var result = mapper.Map<ChatChannel>(channel);
        result.Id = Guid.NewGuid().ToString();
        await DbContext.Channels.AddAsync(result);

        await DbContext.SaveChangesAsync();

        await cache.RemoveAsync(CacheKey);
    }

    /// <summary>
    /// 获取所有的Tags
    /// </summary>
    /// <returns></returns>
    public async Task<string[]> GetTagsAsync()
    {
        var channels = await DbContext.ModelManagers.AsNoTracking().ToArrayAsync();
        var tags = channels.SelectMany(x => x.Tags).Distinct().ToArray();

        return tags;
    }

    /// <summary>
    /// Asynchronously retrieves a paginated list of chat channels based on the provided parameters.
    /// </summary>
    /// <param name="page">The page number to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of items per page. Must be greater than zero.</param>
    /// <param name="keyword">An optional search keyword to filter channels by name. If null or empty, no filtering is applied.</param>
    /// <param name="groups"></param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation, containing a <see cref="PagingDto{T}"/> of <see cref="GetChatChannelDto"/> with the total count and the paginated results.</returns>
    public async ValueTask<PagingDto<GetChatChannelDto>> GetAsync(int page, int pageSize, string? keyword,
        string[]? groups)
    {
        var query = DbContext.Channels.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        if (groups is { Length: > 0 })
        {
            // 如果走分组则需要通过直接查询分组来获取
            var result = await query
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt).ToArrayAsync();

            result = result
                .Where(x => x.Groups.Any(groups.Contains))
                .ToArray();

            var total = result.Length;

            result = result
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            return new PagingDto<GetChatChannelDto>(total,
                mapper.Map<List<GetChatChannelDto>>(result));
        }
        else
        {
            var total = await query.CountAsync();

            if (total <= 0) return new PagingDto<GetChatChannelDto>(total, []);
            {
                var result = await query
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagingDto<GetChatChannelDto>(total, mapper.Map<List<GetChatChannelDto>>(result));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> RemoveAsync(string id)
    {
        var result = await DbContext.Channels.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        await cache.RemoveAsync(CacheKey);

        return result > 0;
    }

    public async ValueTask<ChatChannel> GetAsync(string id)
    {
        var chatChannel = await DbContext.Channels.FindAsync(id);
        if (chatChannel == null) throw new Exception("渠道不存在");

        return chatChannel;
    }

    public async ValueTask<bool> UpdateAsync(string id, ChatChannelInput chatChannel)
    {
        if (string.IsNullOrWhiteSpace(chatChannel.Key))
        {
            var result = await DbContext.Channels.Where(x => x.Id == id)
                .ExecuteUpdateAsync(item =>
                    item.SetProperty(x => x.Type, chatChannel.Type)
                        .SetProperty(x => x.Name, chatChannel.Name)
                        .SetProperty(x => x.Address, chatChannel.Address)
                        .SetProperty(x => x.Groups, chatChannel.Groups)
                        .SetProperty(x => x.Other, chatChannel.Other)
                        .SetProperty(x => x.Extension, chatChannel.Extension)
                        .SetProperty(x => x.Models, chatChannel.Models));
            await cache.RemoveAsync(CacheKey);
            return result > 0;
        }
        else
        {
            var result = await DbContext.Channels.Where(x => x.Id == id)
                .ExecuteUpdateAsync(item =>
                    item.SetProperty(x => x.Type, chatChannel.Type)
                        .SetProperty(x => x.Name, chatChannel.Name)
                        .SetProperty(x => x.Key, chatChannel.Key)
                        .SetProperty(x => x.Address, chatChannel.Address)
                        .SetProperty(x => x.Groups, chatChannel.Groups)
                        .SetProperty(x => x.Extension, chatChannel.Extension)
                        .SetProperty(x => x.Other, chatChannel.Other)
                        .SetProperty(x => x.Models, chatChannel.Models));
            await cache.RemoveAsync(CacheKey);

            return result > 0;
        }
    }

    public async ValueTask UpdateOrderAsync(string id, int order)
    {
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Order, order));

        await cache.RemoveAsync(CacheKey);
    }

    public async ValueTask ControlAutomaticallyAsync(string id)
    {
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ControlAutomatically, a => !a.ControlAutomatically));
    }

    public async ValueTask DisableAsync(string id)
    {
        // 更新状态
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Disable, a => !a.Disable));

        await cache.RemoveAsync(CacheKey);
    }

    /// <summary>
    /// 测试渠道接口
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ChannelException"></exception>
    public async ValueTask<(bool, int)> TestChannelAsync(string id)
    {
        var channel = await DbContext.Channels.FindAsync(id);

        if (channel == null)
        {
            throw new Exception("渠道不存在");
        }

        // 获取渠道指定的实现类型的服务
        var chatCompletionsService = GetKeyedService<IThorChatCompletionsService>(channel.Type);

        if (chatCompletionsService == null)
        {
            throw new Exception("渠道服务不存在");
        }

        var chatRequest = new ThorChatCompletionsRequest()
        {
            TopP = 0.7f,
            Temperature = 0.95f,
            MaxTokens = 1,
            Messages = [ThorChatMessage.CreateUserMessage("hello")]
        };

        var platformOptions = new ThorPlatformOptions
        {
            Address = channel.Address,
            ApiKey = channel.Key,
            Other = channel.Other
        };

        if (channel.Type is OpenAIPlatformOptions.PlatformCode or AzureOpenAIPlatformOptions.PlatformCode)
        {
            // 如果没gpt3.5则搜索是否存在gpt4o模型
            chatRequest.Model = channel.Models?.FirstOrDefault(x =>
                x.StartsWith("gpt-4o-mini", StringComparison.OrdinalIgnoreCase)) ?? channel.Models!.First();

            if (string.IsNullOrEmpty(chatRequest.Model))
            {
                // 获取渠道是否支持gpt-3.5-turbo
                chatRequest.Model = channel.Models.Order()
                    .FirstOrDefault(x => x.StartsWith("gpt-", StringComparison.OrdinalIgnoreCase));
            }
        }
        else if (channel.Type == ClaudiaPlatformOptions.PlatformCode)
        {
            chatRequest.Model =
                channel.Models.FirstOrDefault(x => x.StartsWith("claude", StringComparison.OrdinalIgnoreCase));
        }
        else if (channel.Type == SparkDeskPlatformOptions.PlatformCode)
        {
            chatRequest.Model = channel.Models.FirstOrDefault(x =>
                x.StartsWith("general", StringComparison.OrdinalIgnoreCase) ||
                x.StartsWith("SparkDesk", StringComparison.OrdinalIgnoreCase));
        }
        else if (channel.Type == HunyuanPlatformOptions.PlatformCode)
        {
            chatRequest.Model =
                channel.Models.FirstOrDefault(x => !x.Contains("embedding", StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            chatRequest.Model = channel.Models.First();
        }

        if (string.IsNullOrWhiteSpace(chatRequest.Model))
        {
            chatRequest.Model = channel.Models!.First();
        }

        var token = new CancellationTokenSource();

        var sw = Stopwatch.StartNew();

        var response = await chatCompletionsService.ChatCompletionsAsync(chatRequest,
            platformOptions,
            token.Token);

        sw.Stop();

        // 更新渠道测试响应时间
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResponseTime, sw.ElapsedMilliseconds),
                cancellationToken: token.Token);

        if (!string.IsNullOrWhiteSpace(response.Error?.Message))
        {
            throw new ChannelException(response.Error?.Message);
        }

        if (response.Choices?.Count == 0)
        {
            throw new ChannelException("渠道返回数据为空");
        }

        return (response.Choices?.Count > 0, (int)sw.ElapsedMilliseconds);
    }

    /// <summary>
    /// 从Excel文件批量导入渠道
    /// </summary>
    /// <param name="stream">Excel文件流</param>
    /// <returns>导入成功的渠道数量</returns>
    public async ValueTask<int> ImportChannelsFromExcelAsync(Stream stream)
    {
        // 使用MiniExcel读取Excel文件
        var channels = await stream.QueryAsync<ExcelChatChannelInput>();

        int successCount = 0;

        foreach (var channel in channels)
        {
            try
            {
                // 验证必填字段
                if (string.IsNullOrWhiteSpace(channel.Name) ||
                    string.IsNullOrWhiteSpace(channel.Type) ||
                    channel.Models == null ||
                    !channel.Models.Any())
                {
                    continue; // 跳过无效数据
                }

                var chatChannel = new Thor.Service.Dto.ChatChannelInput
                {
                    Name = channel.Name,
                    Type = channel.Type,
                    Address = channel.Address,
                    Key = channel.Key,
                    SupportsResponses = channel.SupportsResponses,
                    Models = channel.Models.Split(',').Select(x => x.Trim()).ToList(),
                    Other = channel.Other,
                    Groups = channel.Groups.Split(',').Select(x => x.Trim()).ToArray()
                };
                // 创建渠道
                await CreateAsync(chatChannel);
                successCount++;
            }
            catch (Exception)
            {
                // 记录异常但继续处理下一条记录
                continue;
            }
        }

        // 清除缓存
        await cache.RemoveAsync(CacheKey);

        return successCount;
    }

    /// <summary>
    /// 生成渠道导入模板
    /// </summary>
    /// <returns>包含模板数据的内存流</returns>
    public MemoryStream GenerateImportTemplate()
    {
        var memoryStream = new MemoryStream();

        // 创建示例数据
        var templateData = new List<ExcelChatChannelInput>
        {
            new()
            {
                Name = "示例渠道1",
                Type = "OpenAI",
                Address = "https://api.openai.com/v1",
                Key = "sk-xxxxxxxxxxxx",
                Models = "gpt-3.5-turbo,gpt-4",
                Other = "",
                Groups = "default"
            },
        };

        // 使用MiniExcel生成Excel文件
        memoryStream.SaveAsAsync(templateData);

        // 将流位置重置到开始
        memoryStream.Position = 0;

        return memoryStream;
    }

    public sealed class ExcelChatChannelInput
    {
        [ExcelColumnName("渠道名称")] public string Name { get; set; }

        /// <summary>
        /// 根地址
        /// </summary>
        [ExcelColumnName("请求地址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 密钥
        /// </summary>
        [ExcelColumnName("密钥")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 模型
        /// </summary>
        [ExcelColumnName("支持的模型列表使用,分割")]
        public string Models { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnName("权重")]
        public string Other { get; set; } = string.Empty;

        /// <summary>
        /// AI类型
        /// </summary>
        [ExcelColumnName("AI类型（OpenAI|AzureOpenAI）")]
        public string Type { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        /// <returns></returns>
        [ExcelColumnName("分组使用,分割")]
        public string Groups { get; set; }


        /// <summary>
        /// 是否支持Responses
        /// </summary>
        /// <returns></returns>
        [ExcelColumnName("是否支持Responses")]
        public bool SupportsResponses { get; set; } = false;
    }
}