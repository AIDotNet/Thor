using System.Text.Json.Serialization;
using AIDotNet.Abstractions;
using AIDotNet.API.Service;
using AIDotNet.API.Service.BackgroundTask;
using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Infrastructure;
using AIDotNet.API.Service.Options;
using AIDotNet.API.Service.Service;
using AIDotNet.Claudia;
using AIDotNet.Qiansail;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.GetSection(JwtOptions.Name)
    .Get<JwtOptions>();

builder.Configuration.GetSection(OpenAIOptions.Name)
    .Get<OpenAIOptions>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonDateTimeConverter());
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMapster();

builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer xxxxxxxxxxxxxxx\"",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });

        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "FastWiki.ServiceApp",
                Version = "v1",
                Contact = new OpenApiContact { Name = "FastWiki.ServiceApp", }
            });
        foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml"))
            options.IncludeXmlComments(item, true);
        options.DocInclusionPredicate((docName, action) => true);
    }).AddCustomAuthentication()
    .AddMemoryCache()
    .AddHttpContextAccessor()
    .AddTransient<ProductService>()
    .AddTransient<ImageService>()
    .AddTransient<AuthorizeService>()
    .AddTransient<TokenService>()
    .AddTransient<ChatService>()
    .AddTransient<LoggerService>()
    .AddTransient<UserService>()
    .AddTransient<ChannelService>()
    .AddTransient<RedeemCodeService>()
    .AddHostedService<StatisticBackgroundTask>()
    .AddHostedService<LoggerBackgroundTask>();

builder.Services.AddSingleton<IUserContext, DefaultUserContext>()
    .AddOpenAIService()
    .AddSparkDeskService()
    .AddQiansail()
    .AddMetaGLMClientV4()
    .AddClaudia()
    .AddOllamaService();

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

builder.Services.AddDbContext<AIDotNetDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
});

builder.Services.AddDbContext<LoggerDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("LoggerConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
});

var app = builder.Build();

app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();
app.Use((async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Request.Path = "/index.html";
    }

    await next(context);

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/index.html";
        await next(context);
    }
}));

app.Use((async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Request.Path = "/index.html";
    }

    await next(context);

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/index.html";
        await next(context);
    }
}));

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!Directory.Exists("/data"))
{
    Directory.CreateDirectory("/data");
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AIDotNetDbContext>();
await dbContext.Database.MigrateAsync();
var loggerDbContext = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
await loggerDbContext.Database.MigrateAsync();

await SettingService.LoadingSettings(app);

app.MapPost("/api/v1/authorize/token", async (AuthorizeService service, string account, string password) =>
        await service.TokenAsync(account, password))
    .WithGroupName("Token")
    .AddEndpointFilter<ResultFilter>()
    .WithDescription("Get token")
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

token.MapDelete("{id}", async (TokenService service, long id) =>
        await service.RemoveAsync(id))
    .WithDescription("删除Token");

token.MapPut("Disable/{id}", async (TokenService service, long id) =>
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

model.MapGet("/types", ModelService.GetTypes)
    .WithDescription("获取模型类型")
    .WithOpenApi();

model.MapGet("/models",
        ModelService.GetModels)
    .WithDescription("获取模型")
    .WithOpenApi();

#endregion

#region Logger

var logger = app.MapGroup("/api/v1/logger")
    .WithGroupName("Logger")
    .WithTags("Logger")
    .AddEndpointFilter<ResultFilter>()
    .RequireAuthorization();

logger.MapGet(string.Empty,
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
        await service.GetAsync(page, pageSize, keyword))
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

redeemCode.MapPost(string.Empty, async (RedeemCodeService service, RedeemCodeInput input, HttpContext context) =>
        await service.CreateAsync(input, context))
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

redeemCode.MapPut("{id}", async (RedeemCodeService service, long id, RedeemCodeInput input) =>
        await service.UpdateAsync(id, input))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapPut("/enable/{id}", async (RedeemCodeService service, long id) =>
        await service.EnableAsync(id))
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

redeemCode.MapDelete("{id}", async (RedeemCodeService service, long id) =>
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

product.MapPost(string.Empty, async (ProductService service, Product product) =>
        await service.CreateAsync(product))
    .WithDescription("创建产品")
    .WithOpenApi()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        Roles = RoleConstant.Admin
    });

product.MapPut(string.Empty, async ([FromServices] ProductService service, [FromBody] Product product) =>
    await service.UpdateAsync(product))
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
        async ([FromServices] ProductService service, [FromServices] HttpContext context) =>
        await service.PayCompleteCallbackAsync(context))
    .WithDescription("支付回调处理")
    .WithOpenApi()
    .AllowAnonymous();

#endregion

app.MapPost("/v1/chat/completions", async (ChatService service, HttpContext httpContext) =>
        await service.CompletionsAsync(httpContext))
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

await app.RunAsync();