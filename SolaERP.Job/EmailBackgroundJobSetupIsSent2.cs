using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
    internal class EmailBackgroundJobSetupIsSent2 : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(EmailBackgroundJobIsSent2));
            options.AddJob<EmailBackgroundJobIsSent2>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                   .AddTrigger(trigger => trigger
                   .ForJob(jobKey));
                   //.StartAt());
                   //.WithSimpleSchedule(schedule =>
                   //schedule.WithIntervalInMinutes(30).WithRepeatCount(14)));
        }
    }
}
