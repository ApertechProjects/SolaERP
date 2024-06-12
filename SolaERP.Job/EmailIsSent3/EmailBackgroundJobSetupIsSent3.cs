using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent3
{
    internal class EmailBackgroundJobSetupIsSent3 : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailBackgroundJobIsSent3));

            options.AddJob<EmailBackgroundJobIsSent3>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                 .AddTrigger(trigger => trigger
                   .ForJob(jobKey)
                   .WithCronSchedule("0 0/55 9-18 * * ?")
                   .Build());

        }
    }
}
