using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent
{
    public class EmailBackgroundJobSetupIsSent : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            try
            {
                var jobKey = JobKey.Create(nameof(EmailBackgroundJobIsSent));

                options.AddJob<EmailBackgroundJobIsSent>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                    .AddTrigger(trigger => trigger
                    .WithIdentity("when start", "group1")
                    .StartNow()
                    .ForJob(jobKey)
                    .Build());


                // Schedule the job with a cron trigger (every minute between 9 AM and 6 PM)
                options.AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithIdentity("when cron", "group1")
                .WithCronSchedule("0 0/3 9-18 * * ?")
                .Build());
              
                Console.WriteLine("Job Run" + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Job run fail: " + ex.Message);
            }
        }
    }
}
