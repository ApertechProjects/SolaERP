using AutoMapper;
using Microsoft.Extensions.Logging;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.ViewModels;
using SolaERP.Job.EmailIsSent1;
using SolaERP.Job.Enums;
using SolaERP.Job.RFQClose;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
	public class Send : ISend
	{
		private readonly ILogger<EmailIsSentForAssignedBuyer> _logger;
		private readonly IBackgroundMailService _backgroundMailService;
		private readonly IMailService _mailService;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public Send(ILogger<EmailIsSentForAssignedBuyer> logger,
								  IUnitOfWork unitOfWork,
								  IBackgroundMailService backgroundMailService,
								  IMailService mailService, IEmailNotificationService emailNotificationService,
		IMapper mapper)
		{
			_logger = logger;
			Console.WriteLine("constructor started");
			Debug.WriteLine("constructor started");
			_unitOfWork = unitOfWork;
			_backgroundMailService = backgroundMailService;
			_mailService = mailService;
			_mapper = mapper;
			_emailNotificationService = emailNotificationService;
			Console.WriteLine("constructor finsihed");
			Debug.WriteLine("constructor finsihed");
		}
		public async Task SendRequestMails(StatusType statusType)
		{
			try
			{
				Console.WriteLine("Execute started");
				Helper helper = new Helper(_unitOfWork);
				var requestUsers = helper.GetUsersIsSent(Procedure.Request);
				if (requestUsers != null && requestUsers.Count > 0)
				{
					foreach (var user in requestUsers)
					{
						Debug.WriteLine("sended to " + user.UserName);
						Console.WriteLine("sended to " + user.UserName);
						var rowInfoDrafts = helper.GetRowInfosForIsSent(Procedure.Request, user.UserId, statusType);

						var rowInfos = _mapper.Map<HashSet<RowInfo>>(rowInfoDrafts);
						if (rowInfos.Count > 0)
						{
							await _backgroundMailService.SendMailAsync(rowInfos, new Person { email = user.Email, lang = user.Language, userName = user.UserName });
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

		public async Task SendRFQCloseMails()
		{
			RFQMethods methods = new RFQMethods(_unitOfWork);

			var vendors = await methods.GetRFQVendorUsers();
			foreach (var rfq in vendors)
			{
				
				var companyName = await _emailNotificationService.GetCompanyName(rfq.Email);
				VM_RFQClose rfqClose = new VM_RFQClose(rfq.Language, rfq.VendorName)
				{
					CompanyName = companyName,
					Language = (Language)Enum.Parse(typeof(Language), rfq.Language.ToString())
				};


				await _mailService.SendUsingTemplate(rfqClose.Subject, rfqClose,
				rfqClose.TemplateName(), null,
				new List<string> { rfq.Email });

			}

			await methods.UpdateIsSent(vendors.Select(x => x.RFQVendorResponseId).ToList());
		}

		public async Task UpdateRFQStatusToClose()
		{
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = @$"set nocount off update Procurement.RFQMain set Status = 2 where RFQDeadline<getDate()";

				var res = command.ExecuteNonQuery() > 0;

			}
			await _unitOfWork.SaveChangesAsync();
		}

	}
}
