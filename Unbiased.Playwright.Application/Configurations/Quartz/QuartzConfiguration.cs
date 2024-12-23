using Quartz;
using Unbiased.Playwright.Application.Jobs;

namespace Unbiased.Playwright.Application.Configurations.Quartz
{
    public static class QuartzConfiguration
    {
        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator quartz)
        {
            var jobKey = new JobKey("NewsGenerateApiJob");
            quartz.AddJob<ConsumeUnprocessedNewsJob>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .StartNow()
            .WithIdentity("NewsGenerateApiJobTrigger"));
        }
    }
}
