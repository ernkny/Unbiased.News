using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Application.Jobs.Listeners
{
    public class RescheduleJobListener : IJobListener
    {
        public string Name => "RescheduleJobListener";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var scheduler = context.Scheduler;
            var jobKey = context.JobDetail.Key;
            var trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
                .Build();

            await scheduler.ScheduleJob(trigger, cancellationToken);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            if (jobException == null)
            {
                var scheduler = context.Scheduler;
                var jobKey = context.JobDetail.Key;
                var trigger = TriggerBuilder.Create()
                    .ForJob(jobKey)
                    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
                    .Build();

                await scheduler.ScheduleJob(trigger, cancellationToken);
            }
        }
    }
}
