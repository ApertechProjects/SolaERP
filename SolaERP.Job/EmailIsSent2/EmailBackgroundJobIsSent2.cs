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

namespace SolaERP.Job.EmailIsSent2
{

    [DisallowConcurrentExecution]
    public class EmailBackgroundJobIsSent2 : IJob
    {
        private readonly ILogger<EmailBackgroundJobIsSent2> _logger;
        private readonly IBackgroundMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailBackgroundJobIsSent2(ILogger<EmailBackgroundJobIsSent2> logger,
                                         IUnitOfWork unitOfWork,
                                         IBackgroundMailService mailService,
                                         IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await SendMails(StatusType.Other);
            await SendMails(StatusType.AssignedBuyer);
        }

        private async Task SendMails(StatusType statusType)
        {
            try
            {
                Helper helper = new Helper(_unitOfWork);
                var requestUsers = helper.GetUsersIsSent2(Procedure.Request);

                if (requestUsers != null && requestUsers.Count > 0)
                {
                    foreach (var user in requestUsers)
                    {
                        var rowInfoDrafts = helper.GetRowInfosForIsSent2(Procedure.Request, user.UserId, statusType);
                        var rowInfos = _mapper.Map<HashSet<RowInfo>>(rowInfoDrafts);
                        if (rowInfos.Count > 0)
                        {
                            await _mailService.SendMailAsync(rowInfos, new Person { email = user.Email, lang = user.Language, userName = user.UserName });
                            Debug.WriteLine($"sended to {user.UserName}");
                            int[] ids = rowInfoDrafts.Select(x => x.notificationSenderId).ToArray();
                            helper.UpdateIsSent2(ids);
                            _unitOfWork.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the email background job 2.");
                Console.WriteLine("Exception: " + ex.Message);
            }
            await Task.CompletedTask;

        }
    }
}
