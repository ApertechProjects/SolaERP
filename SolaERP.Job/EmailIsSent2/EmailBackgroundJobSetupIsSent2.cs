using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent2
{
    internal class EmailBackgroundJobSetupIsSent2 : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailBackgroundJobIsSent2));

            options.AddJob<EmailBackgroundJobIsSent2>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                   .AddTrigger(trigger => trigger
                   .ForJob(jobKey)
                   .WithCronSchedule("0 0 10 * * ?")
                   .WithSimpleSchedule(schedule =>
                   schedule.WithIntervalInHours(1).WithRepeatCount(3)));

        }
    }
}
