using Quartz;
using Unbiased.Playwright.Application.Jobs;

namespace Unbiased.Playwright.Application.Configurations.Quartz
{
    public static class QuartzConfiguration
    {
        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator quartz)
        {
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");;

            var jobKeyPlaywrightNewsGenerate = new JobKey("PlaywrightNewsGenerateApiJob");
            quartz.AddJob<GetNewsWithPlaywrightWithSearchUrlJob>(opts => opts
                .WithIdentity(jobKeyPlaywrightNewsGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyPlaywrightNewsGenerate)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(10)
                    .RepeatForever())
                .WithIdentity("PlaywrightNewsGenerateApiJob"));

            var jobKeyNewsGenerate = new JobKey("NewsGenerateApiJob");
            quartz.AddJob<ConsumeUnprocessedNewsJob>(opts => opts
                .WithIdentity(jobKeyNewsGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyNewsGenerate)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(5)
                    .RepeatForever())
                .WithIdentity("NewsGenerateApiJob"));

            var jobKeyNewsGenerateImage = new JobKey("NewsGenerateImageApiJob");
            quartz.AddJob<ConsumeUnprocessedImagesJob>(opts => opts
                .WithIdentity(jobKeyNewsGenerateImage)
                .StoreDurably()
                .DisallowConcurrentExecution());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyNewsGenerateImage)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(20)
                    .RepeatForever())
                .WithIdentity("NewsGenerateImageApiJob"));

            var jobKeyHoroscopeGenerate = new JobKey("HoroscopeGenerateJob");

            quartz.AddJob<GetDailyHoroscopeDataJob>(opts => opts
                .WithIdentity(jobKeyHoroscopeGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyHoroscopeGenerate)
                .WithIdentity("HoroscopeGenerateJob")
                .WithCronSchedule("0 20 10 * * ?", cron => cron
                    .InTimeZone(turkeyTimeZone)
                ));

            var jobKeyDailyContentGenerate = new JobKey("DailyContentGenerateJob");

            quartz.AddJob<GetDailyContentDataJob>(opts => opts
                .WithIdentity(jobKeyDailyContentGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyDailyContentGenerate)
                .WithIdentity("DailyContentGenerateJob")
                .WithCronSchedule("0 10 10 * * ?", cron => cron
                    .InTimeZone(turkeyTimeZone)
                ));
        }
    }
}
