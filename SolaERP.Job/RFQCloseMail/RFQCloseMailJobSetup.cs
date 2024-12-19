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
	public class RFQCloseMailJobSetup :
	IConfigureOptions<QuartzOptions>
	{
		public void Configure(QuartzOptions options)
		{
			var jobKey = JobKey.Create(nameof(RFQCloseMailJob));

			options.AddJob<RFQCloseMailJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
				 .AddTrigger(trigger => trigger
				   .ForJob(jobKey)
				   .WithCronSchedule("0 0/1 9-18 * * ?")
				   .Build());

		}
	}
}

