using Quartz;
using Unbiased.Playwright.Application.Jobs;

namespace Unbiased.Playwright.Application.Configurations.Quartz
{
    /// <summary>
    /// Static configuration class for Quartz job scheduler.
    /// Provides centralized configuration for all scheduled jobs in the application.
    /// </summary>
    public static class QuartzConfiguration
    {
        /// <summary>
        /// Configures all scheduled jobs with their respective triggers and schedules.
        /// Sets up jobs for news generation with Playwright, news processing, image generation,
        /// horoscope data collection, and daily content generation.
        /// </summary>
        /// <param name="quartz">The Quartz configurator for registering jobs and triggers</param>
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
                    .WithIntervalInMinutes(20)
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
                    .WithIntervalInMinutes(4)
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
                    .WithIntervalInSeconds(45)
                    .RepeatForever())
                .WithIdentity("NewsGenerateImageApiJob"));

            var jobKeyHoroscopeGenerate = new JobKey("HoroscopeGenerateJob");

            quartz.AddJob<GetDailyHoroscopeDataJob>(opts => opts
                .WithIdentity(jobKeyHoroscopeGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyHoroscopeGenerate)
                .WithIdentity("HoroscopeGenerateJob")
                .WithCronSchedule("0 20 08 * * ?", cron => cron
                    .InTimeZone(turkeyTimeZone)
                ));

            var jobKeyDailyContentGenerate = new JobKey("DailyContentGenerateJob");

            quartz.AddJob<GetDailyContentDataJob>(opts => opts
                .WithIdentity(jobKeyDailyContentGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyDailyContentGenerate)
                .WithIdentity("DailyContentGenerateJob")
                .WithCronSchedule("0 10 08 * * ?", cron => cron
                    .InTimeZone(turkeyTimeZone)
                ));

            var jobKeyContentGenerate = new JobKey("ContentGenerateJob");
            quartz.AddJob<ConsumeUnprocessContentSubheadings>(opts => opts
                .WithIdentity(jobKeyContentGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyContentGenerate)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(45)
                    .RepeatForever())
                .WithIdentity("ContentGenerateJob"));

            var jobKeySubheadingsGenerate = new JobKey("SubheadingsGenerateJob");
            quartz.AddJob<GetContentSubheadingsJob>(opts => opts
                .WithIdentity(jobKeySubheadingsGenerate)
                .StoreDurably());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeySubheadingsGenerate)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(10)
                    .RepeatForever())
                .WithIdentity("SubheadingsGenerateJob"));
        }
    }
}
