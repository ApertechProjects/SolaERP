using Microsoft.Extensions.Options;
using Quartz;

namespace SolaERP.Job.RFQCloseMail
{
	public class RFQDeadLineMailJobSetup : IConfigureOptions<QuartzOptions>
	{
		public void Configure(QuartzOptions options)
		{
			var jobKey = JobKey.Create(nameof(RFQDeadLineMailJob));

			options.AddJob<RFQDeadLineMailJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
				 .AddTrigger(trigger => trigger
				   .ForJob(jobKey)
				   .WithCronSchedule("0 0/55 9-18 * * ?")
				   .Build());

		}
	}
}

