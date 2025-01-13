using Quartz;
using Unbiased.Playwright.Application.Jobs;

namespace Unbiased.Playwright.Application.Configurations.Quartz
{
    public static class QuartzConfiguration
    {
        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator quartz)
        {

            var jobKeyNewsGenerate = new JobKey("NewsGenerateApiJob");
            quartz.AddJob<ConsumeUnprocessedNewsJob>(opts => opts
                .WithIdentity(jobKeyNewsGenerate)
                .StoreDurably()  
                .DisallowConcurrentExecution());  

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyNewsGenerate)  
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)  
                    .RepeatForever())
                .WithIdentity("NewsGenerateApiJobTrigger"));

            var jobKeyNewsGenerateImage = new JobKey("NewsGenerateImageApiJob");
            quartz.AddJob<ConsumeUnprocessedImagesJob>(opts => opts
                .WithIdentity(jobKeyNewsGenerateImage)
                .StoreDurably()
                .DisallowConcurrentExecution());

            quartz.AddTrigger(opts => opts
                .ForJob(jobKeyNewsGenerateImage)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .WithIdentity("NewsGenerateImageApiJobTrigger"));

        }
    }
}
