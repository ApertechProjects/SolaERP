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
    public class EmailBackgroundJobIsSent : IJob
    {
        private readonly ILogger<EmailBackgroundJobIsSent> _logger;
        private readonly IBackgroundMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmailBackgroundJobIsSent(ILogger<EmailBackgroundJobIsSent> logger,
                                  IUnitOfWork unitOfWork,
                                  IBackgroundMailService mailService,
                                  IMapper mapper)
        {

            _logger = logger;
            _logger.LogInformation("constructor started");
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
            _logger.LogInformation("constructor finsihed");

        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Execute started");
                Helper helper = new Helper(_unitOfWork);
                var requestUsers = helper.GetUsersIsSent(Procedure.Request);
                if (requestUsers != null && requestUsers.Count > 0)
                {
                    foreach (var user in requestUsers)
                    {
                        Debug.WriteLine("sended to " + user.UserName);
                        Console.WriteLine("sended to " + user.UserName);
                        var rowInfoDrafts = helper.GetRowInfosForIsSent(Procedure.Request, user.UserId);

                        var rowInfos = _mapper.Map<HashSet<RowInfo>>(rowInfoDrafts);
                        if (rowInfos.Count > 0)
                        {
                            await _mailService.SendMailAsync(rowInfos, new Person { email = user.Email, lang = user.Language, userName = user.UserName });
                            Console.WriteLine("Log: " + "Mail");
                            int[] ids = rowInfoDrafts.Select(x => x.notificationSenderId).ToArray();
                            helper.UpdateIsSent1(ids);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the email background job 1.");
                Console.WriteLine("Exception: " + ex.Message);
            }
            await Task.CompletedTask;

        }
    }
}
