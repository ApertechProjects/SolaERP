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
                options.AddJob<CbarBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey));

                // Add trigger for 10:05 AM
                options.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithIdentity("trigger1", "group1")
                    .WithCronSchedule("0 05 10 * * ?")
                    .Build());

                // Add trigger for 10:35 AM
                options.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithIdentity("trigger2", "group1")
                    .WithCronSchedule("0 35 10 * * ?")
                    .Build());

                // Add trigger for 11:05 AM
                options.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithIdentity("trigger3", "group1")
                    .WithCronSchedule("0 5 11 * * ?")
                    .Build());

                // Add trigger for 11:35 AM
                options.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithIdentity("trigger4", "group1")
                    .WithCronSchedule("0 35 11 * * ?")
                    .Build());

                // Add trigger for 12:05 PM
                options.AddTrigger(trigger => trigger
                    .ForJob(jobKey)
                    .WithIdentity("trigger5", "group1")
                    .WithCronSchedule("0 5 12 * * ?")
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
