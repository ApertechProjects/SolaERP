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
            try
            {
                var jobKey = JobKey.Create(nameof(EmailBackgroundJobIsSent));
                options.AddJob<EmailBackgroundJobIsSent>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                       .AddTrigger(trigger => trigger
                       .ForJob(jobKey)
                       .WithCronSchedule("0 0/5 9-18 * * ?")
                       .Build());

                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
