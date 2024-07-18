using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent3
{
    internal class EmailSetupIsSent3ForAssignedBuyer : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailIsSent3ForAssignedBuyer));

            options.AddJob<EmailIsSent3ForAssignedBuyer>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                 .AddTrigger(trigger => trigger
                   .ForJob(jobKey)
                   .WithCronSchedule("0 0/55 9-18 * * ?")
                   .Build());

        }
    }
}
