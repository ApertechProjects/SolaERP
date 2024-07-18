using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent2
{
    public class EmailSetupIsSent2 : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailIsSent2));

            options.AddJob<EmailIsSent2>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                   .AddTrigger(trigger => trigger
                   .ForJob(jobKey)
                   .WithCronSchedule("0 0/45 9-18 * * ?")
                   .Build());

        }
    }
}
