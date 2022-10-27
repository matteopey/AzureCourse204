using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace FunctionApp
{
    public class TimerTrigger
    {
        [FunctionName("TimerTrigger")]
        public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation(myTimer.ScheduleStatus.Last.ToLongDateString());
        }
    }
}
