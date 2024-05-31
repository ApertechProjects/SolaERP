using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent
{
    public class EmailBackgroundJobSetupIsSent : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailBackgroundJobIsSent));
            options.AddJob<EmailBackgroundJobIsSent>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                   .AddTrigger(trigger => trigger
                   .ForJob(jobKey)
                   .WithCronSchedule("0 0 15 * * ?")
                   .WithSimpleSchedule(schedule =>
                   schedule.WithIntervalInMinutes(30).WithRepeatCount(14)));
        }
    }
}
