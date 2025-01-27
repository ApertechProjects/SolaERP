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
							await _backgroundMailService.SendMailAsync(rowInfos, new Person { email = user.Email, lang = user.Language, userName = user.UserName }, Enums.EmailTemplateKey.ALL_TYPES_ALL_STATUSES);
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
			foreach (var item in vendors)
			{
				await _backgroundMailService.SendMailAsync(null, new Person
				{
					email = item.Email,
					lang = item.Language,
					userName = item.VendorName
				}, Enums.EmailTemplateKey.RFQ_CLOSE);
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
		
		public async Task SendRFQDeadLineMail()
		{
			RFQMethods methods = new RFQMethods(_unitOfWork);
			
			var vendors = await methods.GetRFQVendorUsersMailIsSentDeadLineFalse();
			foreach (var item in vendors)
			{
				await _mailService.SendRFQDeadLineMail(item.UserId,"test subject" , "test body LastDay");
			}
		
			await methods.UpdateMailIsSentDeadLine(vendors.Select(x => x.RFQVendorResponseId).ToList());
		}

		public async Task SendRFQLastDayMail()
		{
			RFQMethods methods = new RFQMethods(_unitOfWork);
		
			var vendors = await methods.GetRFQVendorUsersMailIsSentLastDayFalse(); 
			foreach (var item in vendors)
			{
				await _mailService.SendRFQLastDayMail(item.UserId,"test subject" , "test body LastDay");
			}
		
			await methods.UpdateMailIsSentLastDay(vendors.Select(x => x.RFQVendorResponseId).ToList());
		}

	}
}
