using AutoMapper;
using Microsoft.Extensions.Logging;
using Quartz;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent3
{

    [DisallowConcurrentExecution]
    public class EmailIsSent3ForAssignedBuyer : IJob
    {
        private readonly ISend _send;

        public EmailIsSent3ForAssignedBuyer(ISend send)
        {
            _send = send;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _send.SendRequestMails(StatusType.Other);
        }


    }
}
