using MassTransit;
using Unbiased.Identity.Application;
using Unbiased.Identity.Application.Interfaces;
using Unbiased.Identity.Application.Services;
using Unbiased.Identity.Infrastructure;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("UnbiasedSqlConnection");
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
builder.Services.AddTransient<UnbiasedSqlConnection>(provider => new UnbiasedSqlConnection(connectionString!));

builder.Services.AddScoped<IRoleManagementRepository, RoleManagementRepository>();
builder.Services.AddScoped<IRoleManagementService, RoleManagementService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplication).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IInfrastructure).Assembly));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
