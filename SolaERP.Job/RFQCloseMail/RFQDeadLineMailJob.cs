using Quartz;

namespace SolaERP.Job.RFQCloseMail
{
	[DisallowConcurrentExecution]
	public class RFQDeadLineMailJob : IJob
	{
		private readonly ISend _send;
		public RFQDeadLineMailJob(ISend send)
		{
			_send = send;
		}


		public async Task Execute(IJobExecutionContext context)
		{
			await _send.SendRFQDeadLineMail();
		}
	}
}
