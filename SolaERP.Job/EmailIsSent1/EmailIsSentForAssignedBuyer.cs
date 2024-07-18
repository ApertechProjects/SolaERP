using AutoMapper;
using Quartz;
using Microsoft.Extensions.Logging;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent1
{
    [DisallowConcurrentExecution]
    public class EmailIsSentForAssignedBuyer : IJob
    {
        private readonly ISend _send;
        public EmailIsSentForAssignedBuyer(ISend send)
        {
            _send = send;

            Console.WriteLine("constructor started");
            Debug.WriteLine("constructor started");
            Console.WriteLine("constructor finsihed");
            Debug.WriteLine("constructor finsihed");

        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _send.SendRequestMails(StatusType.AssignedBuyer);
        }


    }
}
