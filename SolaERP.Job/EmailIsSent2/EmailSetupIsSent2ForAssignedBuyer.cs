using Quartz;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent2
{
    public class EmailSetupIsSent2ForAssignedBuyer : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            try
            {
                var jobKey = JobKey.Create(nameof(EmailIsSent2ForAssignedBuyer));

                options.AddJob<EmailIsSent2ForAssignedBuyer>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                    .AddTrigger(trigger => trigger
                    .WithIdentity("when start", "group2")
                    .StartNow()
                    .ForJob(jobKey)
                    .Build());



                options.AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithIdentity("when cron", "group2")
                .WithCronSchedule("0 0/31 5-18 * * ?")
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
