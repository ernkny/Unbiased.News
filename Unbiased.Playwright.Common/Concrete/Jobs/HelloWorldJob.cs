using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Common.Concrete.Jobs
{
    public class HelloWorldJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}: Hello World from Quartz Job!");
            return Task.CompletedTask;
        }
    }
}
