using Quartz;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent1
{
    public class EmailSetupIsSentForAssignedBuyer : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            try
            {
                var jobKey = JobKey.Create(nameof(EmailIsSentForAssignedBuyer));

                options.AddJob<EmailIsSentForAssignedBuyer>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                    .AddTrigger(trigger => trigger
                    .WithIdentity("when start", "group1")
                    .StartNow()
                    .ForJob(jobKey)
                    .Build());



                options.AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithIdentity("when cron", "group1")
                .WithCronSchedule("0 0/30 5-18 * * ?")
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
