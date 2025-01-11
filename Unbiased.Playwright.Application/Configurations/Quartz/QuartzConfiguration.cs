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

        //    var jobKeyForNewsApi = new JobKey("NewsApiJob");
        //    quartz.AddJob<GetNewsWithPlaywrightWithSearchUrlJob>(opts => opts
        //        .WithIdentity(jobKeyForNewsApi)
        //        .StoreDurably()  
        //        .DisallowConcurrentExecution());  

        //    quartz.AddTrigger(opts => opts
        //        .ForJob(jobKeyForNewsApi)  
        //        .StartNow()
        //        .WithSimpleSchedule(x => x
        //            .WithIntervalInSeconds(10)
        //            .RepeatForever())
        //        .WithIdentity("jobKeyForNewsApiJobTrigger"));
        //
        }
    }
}
