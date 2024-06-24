using Quartz;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolaERP.Job.EmailIsSent;

namespace SolaERP.Job.Cbar
{
    public class CbarBackgroundJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            try
            {
                var jobKey = JobKey.Create(nameof(CbarBackgroundJob));

                options.AddJob<CbarBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                       .AddTrigger(trigger => trigger
                       .ForJob(jobKey)
                       .WithCronSchedule("0 0/30 10-13 * * ?")
                       .Build());

                //// Define cron expressions for the specific start times
                //string[] cronExpressions = new string[]
                //{
                //    "0 5 10 * * ?",  // 10:05 AM
                //    "0 35 10 * * ?", // 10:35 AM
                //    "0 5 11 * * ?",  // 11:05 AM
                //    "0 35 11 * * ?", // 11:35 AM
                //    "0 5 12 * * ?",  // 12:05 PM
                //    "0 35 12 * * ?", // 12:35 PM
                //    "0 40 12 * * ?",  // 1:05 PM
                //    "0 35 13 * * ?"  // 1:35 PM
                //};

                //// Create triggers based on the cron expressions
                //foreach (string cronExpression in cronExpressions)
                //{
                //    options.AddTrigger(trigger => trigger
                //        .ForJob(jobKey)
                //        .WithCronSchedule(cronExpression)
                //        .Build());
                //}

                Console.WriteLine("Job Run" + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Job run fail: " + ex.Message);
            }
        }
    }
}
