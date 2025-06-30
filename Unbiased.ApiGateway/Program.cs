using MassTransit;
using Unbiased.ApiGateway.Application.BackgroundServices;
using Unbiased.ApiGateway.Application.Interfaces;
using Unbiased.ApiGateway.Application.Services;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Connections;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.ApiGateway.Infrastructure.DataAccess.Repositories.Concrete;
using Unbiased.Shared.Extensions.Concrete.Loggging;
using Unbiased.Shared.Extensions.Concrete.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("UnbiasedSqlConnection");
builder.Services.AddTransient<UnbiasedSqlConnection>(provider => new UnbiasedSqlConnection(connectionString!));
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IEventAndActivityLog, EventAndActivityLog>();
builder.Services.AddHostedService<ApiKeyRefresherService>();
var corsUrl = builder.Configuration.GetSection("ConnectionStrings:CorsApi").Value;
var corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS") ?? corsUrl;

builder.Services.AddCors(options =>
{
    options.AddPolicy("UnbiasedCorsPolicy", builder =>
    {
        if (!string.IsNullOrEmpty(corsOrigins))
        {
            var origins = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (origins.Length > 0)
            {
                builder.WithOrigins(origins)
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }
            else
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }
        }
        else
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }
    });
});
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("ConnectionStrings:RabbitMqUrl").Value, "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });
});
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.UseCors("UnbiasedCorsPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ApiKeyAuthorizeMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.MapReverseProxy();
app.Run();
