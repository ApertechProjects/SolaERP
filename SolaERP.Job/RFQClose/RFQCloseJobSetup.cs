using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.RFQClose
{
	public class RFQCloseJobSetup : IConfigureOptions<QuartzOptions>
	{
		public void Configure(QuartzOptions options)
		{
			var jobKey = JobKey.Create(nameof(RFQCloseJob));

			options.AddJob<RFQCloseJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
				 .AddTrigger(trigger => trigger
				   .ForJob(jobKey)
				   .WithCronSchedule("0 0/1 9-18 * * ?")
				   .Build());

		}
	}
}
