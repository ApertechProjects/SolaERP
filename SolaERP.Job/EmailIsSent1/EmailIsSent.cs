using AutoMapper;
using Microsoft.Extensions.Logging;
using Quartz;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.EmailIsSent
{
    [DisallowConcurrentExecution]
    public class EmailIsSent : IJob
    {
        private readonly ISend _send;

        public EmailIsSent(ISend send)
        {
            Console.WriteLine("constructor started");
            Debug.WriteLine("constructor started");
            _send = send;
            Console.WriteLine("constructor finsihed");
            Debug.WriteLine("constructor finsihed");

        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _send.SendRequestMails(StatusType.Other);
           
        }

    }
}
