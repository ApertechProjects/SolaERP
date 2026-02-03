using Microsoft.Extensions.Options;
using Quartz;

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
				   .WithCronSchedule("0 0 9-18 * * ?")
				   .Build());
		}
	}
}

