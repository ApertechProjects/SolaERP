using AutoMapper;
using Microsoft.Extensions.Logging;
using Quartz;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.EmailIsSent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.Cbar
{
    [DisallowConcurrentExecution]
    public class CbarBackgroundJob : IJob
    {
        private readonly ILogger<EmailBackgroundJobIsSent> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CbarBackgroundJob(ILogger<EmailBackgroundJobIsSent> logger,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
