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
                    .AddTrigger(trigger => trigger.
                    ForJob(jobKey).
                    WithCronSchedule("0 5,35 7,5,8 * * ?") //+4 timezone - 7,5,8
                    .Build());

                Console.WriteLine("Job Run" + DateTime.Now.AddHours(4));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Job run fail: " + ex.Message);
            }
        }
    }
}
