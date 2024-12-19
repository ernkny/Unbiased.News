using Quartz;
using Unbiased.Playwright.Common.Concrete.Jobs;

namespace Unbiased.Playwright.Application.Configurations.Quartz
{
    public static class QuartzConfiguration
    {
        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator quartz)
        {
            var jobKey = new JobKey("HelloWorldJob");
            quartz.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts => opts
                .ForJob(jobKey) 
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .WithIdentity("HelloWorldTrigger"));
        }
    }
}
