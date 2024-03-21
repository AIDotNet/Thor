using AIDotNet.API.Service;
using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domina;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Infrastructure;
using AIDotNet.API.Service.Options;
using AIDotNet.API.Service.Service;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TokenApi.Contract.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.GetSection(JwtOptions.Name)
    .Get<JwtOptions>();

builder.Configuration.GetSection(OpenAIOptions.Name)
    .Get<OpenAIOptions>();

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
}).AddJwtBearerAuthentication();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthorizeService>()
    .AddTransient<TokenService>()
    .AddTransient<ChatService>()
    .AddTransient<ChannelService>();

builder.Services.AddSingleton<IUserContext, DefaultUserContext>()
    .AddAliyunFCService()
    .AddSparkDeskService()
    .AddMetaGLMClientV4();

builder.Services.AddDbContext<TokenApiDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TokenApiDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapPost("/api/v1/authorize/token", async (AuthorizeService service, string account, string password) =>
        await service.TokenAsync(account, password))
    .WithGroupName("Token")
    .WithDescription("Get token")
    .WithTags("Authorize")
    .WithOpenApi();

#region Token

var token = app.MapGroup("/api/v1/token")
    .WithGroupName("Token")
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

token.MapGet("list", async (TokenService service, int page, int size) =>
        await service.GetListAsync(page, size))
    .WithDescription("获取Token列表")
    .WithOpenApi();

token.MapPut(string.Empty, async (TokenService service, Token input) =>
        await service.UpdateAsync(input))
    .WithDescription("更新Token");

token.MapDelete("{id}", async (TokenService service, long id) =>
        await service.RemoveAsync(id))
    .WithDescription("删除Token");

#endregion

#region Channel

var channel = app.MapGroup("/api/v1/channel")
    .WithGroupName("Channel")
    .WithTags("Channel")
    .RequireAuthorization();

channel.MapPost("", async (ChannelService service, ChatChannelInput input) =>
    await service.CreateAsync(input));

channel.MapGet("list", async (ChannelService service, int page, int pageSize) =>
    await service.GetAsync(page, pageSize));

channel.MapDelete("{id}", async (ChannelService service, string id) =>
    await service.RemoveAsync(id));

channel.MapGet("{id}", async (ChannelService service, string id) =>
    await service.GetAsync(id));

channel.MapPut(string.Empty, async (ChannelService service, ChatChannel input) =>
    await service.UpdateAsync(input));

channel.MapGet("/model-services", async (ChannelService services) =>
    services.GetModelServices());

#endregion

app.MapPost("/v1/chat/completions", async (ChatService service, HttpContext httpContext) =>
        await service.CompletionsAsync(httpContext))
    .WithGroupName("OpenAI")
    .WithDescription("Get completions from OpenAI")
    .WithOpenApi();

await app.RunAsync();