using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Unbiased.Dashboard.Application;
using Unbiased.Dashboard.Application.Interfaces;
using Unbiased.Dashboard.Application.Services;
using Unbiased.Dashboard.Application.Validators;
using Unbiased.Dashboard.Domain.Model.Aws;
using Unbiased.Dashboard.Infrastructure;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete;
using Unbiased.Shared.Dtos.Concrete.Configurations;
using Unbiased.Shared.Extensions.Concrete.Extensions;
using Unbiased.Shared.Extensions.Concrete.Middlewares;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplication).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IInfrastructure).Assembly));
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IEngineService, EngineService>();
builder.Services.AddScoped<IEngineRepository, EngineRepository>();
builder.Services.AddScoped<AwsCredentials>();
builder.Services.AddCustomTokenAuth(builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>()!);
builder.Services.Configure<AwsCredentials>(builder.Configuration.GetSection("S3Settings"));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateGeneratedNewsWithImageDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InsertNewsWithImageDtoValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<GlobalActivityLogMiddleware>();
app.UseMiddleware<ApiKeyAuthorizeMiddleware>();
app.MapControllers();
app.Run();
