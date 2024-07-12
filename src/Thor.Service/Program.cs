using System.Text.Json.Serialization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Thor.BuildingBlocks.Data;
using Thor.Claudia;
using Thor.LocalEvent;
using Thor.LocalMemory.Cache;
using Thor.Moonshot;
using Thor.Qiansail;
using Thor.RedisMemory.Cache;
using Thor.Service;
using Thor.Service.BackgroundTask;
using Thor.Service.EventBus;
using Thor.Service.Filters;
using Thor.Service.Infrastructure;
using Thor.Service.Infrastructure.Middlewares;
using Thor.Service.Options;
using Thor.Service.Service;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.HostEnvironment();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonDateTimeConverter());
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMapster();

if (string.IsNullOrEmpty(CacheOptions.Type) ||
    CacheOptions.Type.Equals("Memory", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddLocalMemory();
}
else if (CacheOptions.Type.Equals("Redis", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddRedisMemory(CacheOptions.ConnectionString);
}

builder.Services.AddMvcCore().AddApiExplorer();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddSingleton<IEventHandler<ChatLogger>, ChannelEventHandler>()
    .AddLocalEventBus()
    .AddCustomAuthentication()
    .AddHttpContextAccessor()
    .AddTransient<ProductService>()
    .AddTransient<ImageService>()
    .AddTransient<AuthorizeService>()
    .AddTransient<TokenService>()
    .AddTransient<SystemService>()
    .AddTransient<ChatService>()
    .AddTransient<LoggerService>()
    .AddTransient<UserService>()
    .AddTransient<ChannelService>()
    .AddTransient<RedeemCodeService>()
    .AddTransient<RateLimitModelService>()
    .AddHostedService<StatisticBackgroundTask>()
    .AddHostedService<LoggerBackgroundTask>()
    .AddHostedService<AutoChannelDetectionBackgroundTask>()
    .AddSingleton<UnitOfWorkMiddleware>()
    .AddSingleton<IUserContext, DefaultUserContext>()
    .AddOpenAIService()
    .AddMoonshotService()
    .AddSparkDeskService()
    .AddQiansail()
    .AddMetaGLMClientV4()
    .AddHunyuan()
    .AddClaudia()
    .AddOllamaService()
    .AddAzureOpenAIService()
    .AddErnieBot();

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

// 获取环境变量
var dbType = Environment.GetEnvironmentVariable("DBType");

dbType ??= builder.Configuration["ConnectionStrings:DBType"];

var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
var loggerConnectionString = Environment.GetEnvironmentVariable("LoggerConnectionString");

connectionString ??= builder.Configuration.GetConnectionString("DefaultConnection");
loggerConnectionString ??= builder.Configuration.GetConnectionString("LoggerConnection");

if (string.IsNullOrEmpty(dbType) || string.Equals(dbType, "sqlite"))
{
    builder.Services.AddDbContext<AIDotNetDbContext>(options =>
    {
        options.UseSqlite(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });

    builder.Services.AddDbContext<LoggerDbContext>(options =>
    {
        options.UseSqlite(loggerConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });
}
else if (string.Equals(dbType, "postgresql") || string.Equals(dbType, "pgsql"))
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

    builder.Services.AddDbContext<AIDotNetDbContext>(options =>
    {
        options.UseNpgsql(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });

    builder.Services.AddDbContext<LoggerDbContext>(options =>
    {
        options.UseNpgsql(loggerConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });
}
else if (string.Equals(dbType, "sqlserver") || string.Equals(dbType, "mssql"))
{
    builder.Services.AddDbContext<AIDotNetDbContext>(options =>
    {
        options.UseSqlServer(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });

    builder.Services.AddDbContext<LoggerDbContext>(options =>
    {
        options.UseSqlServer(loggerConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });
}
else if (string.Equals(dbType, "mysql"))
{
    builder.Services.AddDbContext<AIDotNetDbContext>(options =>
    {
        options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });

    builder.Services.AddDbContext<LoggerDbContext>(options =>
    {
        options.UseMySql(loggerConnectionString,
                ServerVersion.AutoDetect(loggerConnectionString))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    });
}
else
{
    throw new Exception("不支持的数据库类型");
}

builder.Services.AddResponseCompression();

var app = builder.Build();

app.MapDefaultEndpoints();

using var scope = app.Services.CreateScope();

if (string.IsNullOrEmpty(dbType) || string.Equals(dbType, "sqlite"))
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AIDotNetDbContext>();
    // 不使用迁移记录生成
    await dbContext.Database.EnsureCreatedAsync();

    await RateLimitModelService.LoadAsync(dbContext);

    var loggerDbContext = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
    await loggerDbContext.Database.EnsureCreatedAsync();
}
// 由于没有生成迁移记录，所以使用EnsureCreated
else if (string.Equals(dbType, "postgresql") || string.Equals(dbType, "pgsql") || string.Equals(dbType, "sqlserver") ||
         string.Equals(dbType, "mssql"))
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AIDotNetDbContext>();
    // 不使用迁移记录生成
    await dbContext.Database.EnsureCreatedAsync();

    await RateLimitModelService.LoadAsync(dbContext);

    var loggerDbContext = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
    await loggerDbContext.Database.EnsureCreatedAsync();
}

await SettingService.LoadingSettings(app);

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.Use((async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        if (string.IsNullOrEmpty(ChatCoreOptions.Master))
        {
            context.Request.Path = "/index.html";
        }
        else
        {
            context.Response.Redirect(ChatCoreOptions.Master);
            return;
        }
    }

    context.Response.Headers["AI-Gateway-Versions"] = "1.0.0.1";
    context.Response.Headers["AI-Gateway-Name"] = "AI-Gateway";

    await next(context);

    if (context.Response.StatusCode == 404)
    {
        if (string.IsNullOrEmpty(ChatCoreOptions.Master))
        {
            context.Request.Path = "/index.html";
        }
        else
        {
            context.Response.Redirect(ChatCoreOptions.Master);
            return;
        }

        await next(context);
    }
}));

app.UseResponseCompression();

var theme = (Environment.GetEnvironmentVariable("Theme") ??
             SettingService.GetSetting(SettingExtensions.SystemSetting.Theme));
if (theme == "default" || string.IsNullOrEmpty(theme))
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(path),
    });
}
else
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), theme))
    });
}

app.UseMiddleware<UnitOfWorkMiddleware>();

if (!Directory.Exists("/data"))
{
    Directory.CreateDirectory("/data");
}


app.MapPost("/api/v1/authorize/token", async (AuthorizeService service, [FromBody] LoginInput input) =>
    await service.TokenAsync(input))
    .WithGroupName("Token")
    .AddEndpointFilter<ResultFilter>()
    .WithDescription("Get token")
    .WithTags("Authorize")
    .WithOpenApi();

app.MapPost("/api/v1/authorize/github", async (AuthorizeService service, string code) =>
        await service.GithubAsync(code))
    .WithGroupName("Token")
    .AddEndpointFilter<ResultFilter>()
    .WithDescription("Github login")
    .WithTags("Authorize")
    .WithOpenApi();


#region Token

var token = app.MapGroup("/api/v1/token")
    .WithGroupName("Token")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization()
    .WithTags("Token");

token.MapPost(string.Empty, async (TokenService service, TokenInput input) =>
        await service.CreateAsync(input))
    .WithDescription("创建Token")
    .WithOpenApi();

token.MapGet("{id}", async (TokenService service, long id) =>
        await service.GetAsync(id))
    .WithDescription("获取Token详情")
    .WithOpenApi();

token.MapGet("list", async (TokenService service, int page, int pageSize, string? token, string? keyword) =>
        await service.GetListAsync(page, pageSize, token, keyword))
    .WithDescription("获取Token列表")
    .WithOpenApi();

token.MapPut(string.Empty, async (TokenService service, Token input) =>
        await service.UpdateAsync(input))
    .WithDescription("更新Token");

token.MapDelete("{id}", async (TokenService service, string id) =>
        await service.RemoveAsync(id))
    .WithDescription("删除Token");

token.MapPut("Disable/{id}", async (TokenService service, string id) =>
        await service.DisableAsync(id))
    .WithDescription("禁用Token");

#endregion

#region Channel

var channel = app.MapGroup("/api/v1/channel")
    .WithGroupName("Channel")
    .WithTags("Channel")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

channel.MapPost("", async (ChannelService service, ChatChannelInput input) =>
    await service.CreateAsync(input));

channel.MapGet("list", async (ChannelService service, int page, int pageSize) =>
    await service.GetAsync(page, pageSize));

channel.MapDelete("{id}", async (ChannelService service, string id) =>
    await service.RemoveAsync(id));

channel.MapGet("{id}", async (ChannelService service, string id) =>
    await service.GetAsync(id));

channel.MapPut("{id}", async (ChannelService service, ChatChannelInput input, string id) =>
    await service.UpdateAsync(id, input));

channel.MapPut("/disable/{id}", async (ChannelService services, string id) =>
    await services.DisableAsync(id));

channel.MapPut("/test/{id}", async (ChannelService services, string id) =>
    await services.TestChannelAsync(id));

channel.MapPut("/control-automatically/{id}", async (ChannelService services, string id) =>
    await services.ControlAutomaticallyAsync(id));

channel.MapPut("/order/{id}", async (ChannelService services, string id, int order) =>
    await services.UpdateOrderAsync(id, order));

#endregion

#region Model

var model = app.MapGroup("/api/v1/model")
    .WithGroupName("Model")
    .WithTags("Model")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization();

model.MapGet("/types", ModelService.GetPlatformNames)
    .WithDescription("获取模型类型")
    .WithOpenApi();

model.MapGet("/models",
        ModelService.GetModels)
    .WithDescription("获取模型")
    .WithOpenApi();

model.MapGet("/use-models", async (HttpContext context) => { return await ModelService.GetUseModels(context); })
    .WithDescription("获取使用模型")
    .AllowAnonymous()
    .WithOpenApi();

#endregion

#region Logger

var log = app.MapGroup("/api/v1/logger")
    .WithGroupName("Logger")
    .WithTags("Logger")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization();

log.MapGet(string.Empty,
        async (LoggerService service, int page, int pageSize, ChatLoggerType? type, string? model, DateTime? startTime,
                DateTime? endTime, string? keyword) =>
            await service.GetAsync(page, pageSize, type, model, startTime, endTime, keyword))
    .WithDescription("获取日志")
    .WithDisplayName("获取日志")
    .WithOpenApi();

#endregion

#region User

var user = app.MapGroup("/api/v1/user")
    .WithGroupName("User")
    .WithTags("User")
    .AddEndpointFilter<ResultFilter>();

user.MapPost(string.Empty, async (UserService service, CreateUserInput input) =>
        await service.CreateAsync(input))
    .AllowAnonymous();

user.MapGet(string.Empty, async (UserService service, int page, int pageSize, string? keyword) =>
        await service.GetListAsync(page, pageSize, keyword))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

user.MapGet("info", async (UserService service) =>
        await service.GetAsync())
    .RequireAuthorization();


user.MapDelete("{id}", async (UserService service, string id) =>
        await service.RemoveAsync(id))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

user.MapPut(string.Empty, async (UserService service, UpdateUserInput input) =>
        await service.UpdateAsync(input))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

user.MapPost("/enable/{id}", async (UserService service, string id) =>
        await service.EnableAsync(id))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

user.MapPut("/update-password", async (UserService service, UpdatePasswordInput input) =>
        await service.UpdatePasswordAsync(input))
    .RequireAuthorization();

#endregion

#region Redeem Code

var redeemCode = app.MapGroup("/api/v1/redeemCode")
    .WithGroupName("RedeemCode")
    .WithTags("RedeemCode")
    .AddEndpointFilter<ResultFilter>();

redeemCode.MapPost(string.Empty, async (RedeemCodeService service, RedeemCodeInput input) =>
        await service.CreateAsync(input))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapGet(string.Empty, async (RedeemCodeService service, int page, int pageSize, string? keyword) =>
        await service.GetAsync(page, pageSize, keyword))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapPut("{id}", async (RedeemCodeService service, string id, RedeemCodeInput input) =>
        await service.UpdateAsync(id, input))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapPut("/enable/{id}", async (RedeemCodeService service, string id) =>
        await service.EnableAsync(id))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapDelete("{id}", async (RedeemCodeService service, string id) =>
        await service.RemoveAsync(id))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapPost("/use/{code}", async (RedeemCodeService service, string code) =>
        await service.UseAsync(code))
    .RequireAuthorization(new AuthorizeAttribute());

#endregion

#region Setting

var setting = app.MapGroup("/api/v1/setting")
    .WithGroupName("Setting")
    .WithTags("Setting")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

setting.MapGet(string.Empty, SettingService.GetSettings)
    .WithDescription("获取设置")
    .AllowAnonymous()
    .WithOpenApi();

setting.MapPut(string.Empty, SettingService.UpdateSettingsAsync)
    .WithDescription("更新设置")
    .WithOpenApi();

#endregion

#region Statistics

var statistics = app.MapGroup("/api/v1/statistics")
    .WithGroupName("Statistics")
    .WithTags("Statistics")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization();

statistics.MapGet(string.Empty,
    async ([FromServices] AIDotNetDbContext dbContext,
            [FromServices] LoggerDbContext loggerDbContext,
            [FromServices] IUserContext userContext) =>
        await StatisticsService.GetStatisticsAsync(loggerDbContext, dbContext, userContext));

#endregion

#region Product

var product = app.MapGroup("/api/v1/product")
    .WithGroupName("Product")
    .WithTags("Product")
    .AddEndpointFilter<ResultFilter>();

product.MapGet(string.Empty, async (ProductService service) =>
        await service.GetProductsAsync())
    .WithDescription("获取产品列表")
    .WithOpenApi()
    .RequireAuthorization();

product.MapPost(string.Empty,  (ProductService service, Product product) =>
        service.Create(product))
    .WithDescription("创建产品")
    .WithOpenApi()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

product.MapPut(string.Empty,  ([FromServices] ProductService service, [FromBody] Product product) =>
    service.Update(product))
    .WithDescription("更新产品")
    .WithOpenApi()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

product.MapDelete("{id}", async ([FromServices] ProductService service, string id) =>
    await service.DeleteAsync(id))
    .WithDescription("删除产品")
    .WithOpenApi()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

product.MapPost("start-pay-payload/{id}", async ([FromServices] ProductService service, string id) =>
    await service.StartPayPayloadAsync(id))
    .WithDescription("发起支付")
    .WithOpenApi()
    .RequireAuthorization();

product.MapPost("pay-complete-callback",
        async ([FromServices] ProductService service, HttpContext context) =>
        await service.PayCompleteCallbackAsync(context))
    .WithDescription("支付回调处理")
    .WithOpenApi()
    .AllowAnonymous();

#endregion

#region 模型限流策略

var rateLimitModel = app.MapGroup("/api/v1/rateLimitModel")
    .WithGroupName("RateLimitModel")
    .WithTags("RateLimitModel")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

rateLimitModel.MapGet(string.Empty, async (RateLimitModelService service, int page, int pageSize) =>
        await service.GetAsync(page, pageSize))
    .WithDescription("获取限流策略")
    .WithOpenApi();

rateLimitModel.MapPost(string.Empty, async (RateLimitModelService service, RateLimitModel rateLimitModel) =>
        await service.CreateAsync(rateLimitModel))
    .WithDescription("创建限流策略")
    .WithOpenApi();

rateLimitModel.MapPut(string.Empty, async (RateLimitModelService service, RateLimitModel rateLimitModel) =>
        await service.UpdateAsync(rateLimitModel))
    .WithDescription("更新限流策略")
    .WithOpenApi();

rateLimitModel.MapDelete("{id}", async (RateLimitModelService service, string id) =>
        await service.RemoveAsync(id))
    .WithDescription("删除限流策略")
    .WithOpenApi();

rateLimitModel.MapPut("/disable/{id}", async (RateLimitModelService service, string id) =>
        await service.Disable(id))
    .WithDescription("禁用|启用限流策略")
    .WithOpenApi();

#endregion

#region System

var system = app.MapGroup("/api/v1/system")
    .WithGroupName("System")
    .WithTags("System")
    .AddEndpointFilter<ResultFilter>();

system.MapPost("share", async (SystemService service, string userId, HttpContext context) =>
        await service.ShareAsync(userId, context))
    .WithDescription("触发分享获取奖励")
    .WithOpenApi();

#endregion

// 对话补全请求
app.MapPost("/v1/chat/completions", async (ChatService service, HttpContext httpContext) =>
        await service.ChatCompletionsAsync(httpContext))
    .WithGroupName("OpenAI")
    .WithDescription("Get completions from OpenAI")
    .WithOpenApi();

// 文本补全接口
app.MapPost("/v1/completions", async (ChatService service, HttpContext context) =>
        await service.CompletionsAsync(context))
    .WithGroupName("OpenAI")
    .WithDescription("Get completions from OpenAI")
    .WithOpenApi();

app.MapPost("/v1/embeddings", async (ChatService embeddingService, HttpContext context) =>
        await embeddingService.EmbeddingAsync(context))
    .WithDescription("OpenAI")
    .WithDescription("Embedding")
    .WithOpenApi();

app.MapPost("/v1/images/generations", async (ChatService imageService, HttpContext context) =>
        await imageService.ImageAsync(context))
    .WithDescription("OpenAI")
    .WithDescription("Image")
    .WithOpenApi();


app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();