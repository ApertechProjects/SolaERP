using Microsoft.Extensions.Options;
using Quartz;
using SolaERP.Job.EmailIsSent3;
using SolaERP.Job.RFQClose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.RFQCloseMail
{
	public class RFQLastDayMailJobSetup : IConfigureOptions<QuartzOptions>
	{
		public void Configure(QuartzOptions options)
		{
			var jobKey = JobKey.Create(nameof(RFQLastDayMailJob));

			options.AddJob<RFQLastDayMailJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
				 .AddTrigger(trigger => trigger
				   .ForJob(jobKey)
				   .WithCronSchedule("0 0/25 9-18 * * ?")
				   .Build());

		}
	}
}

