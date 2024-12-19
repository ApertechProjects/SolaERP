using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.RFQClose
{
	[DisallowConcurrentExecution]
	public class RFQCloseJob : IJob
	{
		private readonly ISend _send;
        public RFQCloseJob( ISend send)
        {
			_send = send;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			await _send.UpdateRFQStatusToClose();
		}
	}
}
