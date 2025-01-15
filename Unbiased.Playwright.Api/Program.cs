using MassTransit;
using Polly;
using Polly.Retry;
using Quartz;
using Unbiased.Playwright.Application;
using Unbiased.Playwright.Application.Configurations.Quartz;
using Unbiased.Playwright.Application.Configurations.Startup;
using Unbiased.Playwright.Application.Interfaces;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Application.Jobs.Listeners;
using Unbiased.Playwright.Application.Services;
using Unbiased.Playwright.Infrastructure;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete;
using Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var pipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions())
    .AddTimeout(TimeSpan.FromSeconds(10))
    .Build();
await pipeline.ExecuteAsync(static async token => { await Task.Delay(1000, token); }, CancellationToken.None);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5001")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("UnbiasedSqlConnection");
builder.Services.AddTransient<UnbiasedSqlConnection>(provider => new UnbiasedSqlConnection(connectionString!));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplication).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IInfrastructure).Assembly));
builder.Services.AddSingleton<IJobListener, RescheduleJobListener>(); 
builder.Services.AddSingleton<IJobListener, RescheduleJobListenerForImage>(); 
builder.Services.AddSingleton<IJobListener, RescheduleJobListenerForHoroscope>(); 
builder.Services.AddQuartz(q =>
{
    q.SchedulerName = "MyScheduler";

    q.UsePersistentStore(s =>
    {
        s.UseProperties = true;
        s.RetryInterval = TimeSpan.FromSeconds(15);
        s.UseSqlServer(sqlServer =>
        {
            sqlServer.ConnectionString = connectionString!;
        });
        s.UseSystemTextJsonSerializer();
        s.UseClustering(c =>
        {
            c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
            c.CheckinInterval = TimeSpan.FromSeconds(10);
        });

    });
    QuartzConfiguration.ConfigureJobs(q);
    q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 10);
    q.UseSimpleTypeLoader();
});
builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
});
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsImageRepository, NewsImageRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ISearchUrlRepository, SearchUrlRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IPlaywrightScrappingService, PlaywrightScrappingService>();
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
builder.Services.AddHostedService<UpdateScrappingRunTimes>();
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
