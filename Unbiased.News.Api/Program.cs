using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Unbiased.News.Application;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Application.Services;
using Unbiased.News.Application.Validators;
using Unbiased.News.Infrastructure;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete;
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5001")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplication).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IInfrastructure).Assembly));
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesService,CategoriesService>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddCustomTokenAuth(builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>()!);
builder.Services.AddValidatorsFromAssemblyContaining<ContactValidator>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
