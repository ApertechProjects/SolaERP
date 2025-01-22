using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.Cbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.RFQCloseMail
{
	[DisallowConcurrentExecution]
	public class RFQLastDayMailJob : IJob
	{
		private readonly ISend _send;
		public RFQLastDayMailJob(ISend send)
		{
			_send = send;
		}


		public async Task Execute(IJobExecutionContext context)
		{
			await _send.SendRFQLastDayMail();
		}
	}
}
