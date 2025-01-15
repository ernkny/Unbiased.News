using Quartz;

namespace Unbiased.Playwright.Application.Jobs.Listeners
{
    public class RescheduleJobListenerForDailyContent:IJobListener
    {
        public string Name => "RescheduleJobListenerForDailtyContent";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var scheduler = context.Scheduler;
            var jobKey = context.JobDetail.Key;
            var trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .WithCronSchedule("0 0 9 * * ?")
                .Build();

            await scheduler.ScheduleJob(trigger, cancellationToken);
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            if (jobException == null)
            {
                var scheduler = context.Scheduler;
                var jobKey = context.JobDetail.Key;
                var trigger = TriggerBuilder.Create()
                    .ForJob(jobKey)
                    .WithCronSchedule("0 0 9 * * ?")
                    .Build();

                await scheduler.ScheduleJob(trigger, cancellationToken);
            }
        }
    }
}
