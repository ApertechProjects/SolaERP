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

        public async Task Execute(IJobExecutionContext context)
        {
            Helper helper = new Helper(_unitOfWork);
            var requestUsers = await helper.GetUsersIsSent3(Procedure.Request);

            if (requestUsers != null && requestUsers.Count > 0)
            {
                foreach (var user in requestUsers)
                {
                    var rowInfoDrafts = await helper.GetRowInfosForIsSent3(Procedure.Request, user.UserId);
                    var rowInfos = _mapper.Map<HashSet<RowInfo>>(rowInfoDrafts);
                    if (rowInfos.Count > 0)
                    {
                        _mailService.SendMail(rowInfos, new Person { email = user.Email, lang = user.Language, userName = user.UserName });
                        Debug.WriteLine($"sended to {user.UserName}");
                        int[] ids = rowInfoDrafts.Select(x => x.notificationSenderId).ToArray();
                        await helper.UpdateIsSent3(ids);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

            }
        }
    }
}
