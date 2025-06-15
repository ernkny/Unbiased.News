using MassTransit;
using Unbiased.Log.Application;
using Unbiased.Log.Application.Interfaces;
using Unbiased.Log.Application.MessageQuery.Rabbitmq.Masstransit.Concrete;
using Unbiased.Log.Application.Services;
using Unbiased.Log.Infrastructure;
using Unbiased.Log.Infrastructure.DataAccess.Connections;
using Unbiased.Log.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Log.Infrastructure.DataAccess.Repositories.Concrete;
using Unbiased.Shared.Extensions.Concrete.Loggging;
using Unbiased.Shared.Extensions.Concrete.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var connectionString = builder.Configuration.GetConnectionString("UnbiasedSqlConnection");
builder.Services.AddTransient<UnbiasedSqlConnection>(provider => new UnbiasedSqlConnection(connectionString!)); builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplication).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IInfrastructure).Assembly));
builder.Services.AddScoped<IEventAndActivityLog, EventAndActivityLog>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EventLogConsumer>();
    x.AddConsumer<ActivityLogConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("ConnectionStrings:RabbitMqUrl").Value, "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        cfg.ReceiveEndpoint("EventLogMessageQueue", e =>
        {
            e.ConfigureConsumer<EventLogConsumer>(context);
        });
        cfg.ReceiveEndpoint("ActivityLogMessageQueue", e =>
        {
            e.ConfigureConsumer<ActivityLogConsumer>(context);
        });
    });
});
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();
var app = builder.Build();

app.UseCors("MyCorsPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<GlobalActivityLogMiddleware>();
app.UseMiddleware<ApiKeyAuthorizeMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
