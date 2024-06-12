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
    public class EmailBackgroundJobIsSent3 : IJob
    {
        private readonly ILogger<EmailBackgroundJobIsSent3> _logger;
        private readonly IBackgroundMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailBackgroundJobIsSent3(ILogger<EmailBackgroundJobIsSent3> logger,
                                         IUnitOfWork unitOfWork,
                                         IBackgroundMailService mailService,
                                         IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Helper helper = new Helper(_unitOfWork);
                var requestUsers = helper.GetUsersIsSent3(Procedure.Request);

                if (requestUsers != null && requestUsers.Count > 0)
                {
                    foreach (var user in requestUsers)
                    {
                        var rowInfoDrafts = helper.GetRowInfosForIsSent3(Procedure.Request, user.UserId);
                        var rowInfos = _mapper.Map<HashSet<RowInfo>>(rowInfoDrafts);
                        if (rowInfos.Count > 0)
                        {
                            _mailService.SendMail(rowInfos, new Person { email = user.Email, lang = user.Language, userName = user.UserName });
                            int[] ids = rowInfoDrafts.Select(x => x.notificationSenderId).ToArray();
                            helper.UpdateIsSent3(ids);
                            _unitOfWork.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the email background job 3.");
                Console.WriteLine("Exception: " + ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
