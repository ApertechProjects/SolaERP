using Confluent.Kafka;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using SolaERP.Job.RFQClose;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SolaERP.Application.Contracts.Services;
using static SolaERP.Job.Helper;

namespace SolaERP.Job
{
	public class RFQMethods
	{
		private readonly IUnitOfWork _unitOfWork;
		public RFQMethods(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<RFQUserData>> GetRFQVendorUsers()
		{
			List<RFQUserData> userDatas = new();

			var text = "select vr.RFQVendorResponseId,vr.RFQMainId,au.Language,vr.VendorCode,v.VendorName,au.Email from Procurement.RFQVendorResponse vr INNER JOIN Procurement.Vendors v ON vr.VendorCode = v.VendorCode INNER JOIN Config.AppUser au ON v.VendorId = au.VendorId INNER JOIN Procurement.RFQMain rm ON vr.RFQMainId = rm.RFQMainId where vr.MailIsSent = 0 and rm.RFQDeadline < GETDATE() GROUP BY vr.RFQVendorResponseId,vr.RFQMainId,au.Language,vr.VendorCode,v.VendorName,au.Email";

			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = text;

				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					userDatas.Add(new RFQUserData
					{
						RFQMainId = reader.Get<int>("RFQMainId"),
						Email = reader.Get<string>("Email"),
						Language = reader.Get<string>("Language"),
						VendorCode = reader.Get<string>("VendorCode"),
						VendorName = reader.Get<string>("VendorName")
					});
				}
				return userDatas;
			}
		}
		
		public async Task<List<RFQUserData>> GetRFQVendorUsersMailIsSentDeadLineFalse()
		{
			List<RFQUserData> userDatas = new();

			var text = @"
				select
    			vr.RFQVendorResponseId,
    			vr.RFQMainId,
    			au.Language,
			    vr.VendorCode,
			    v.VendorName,
			    au.Email,
			    au.Id as UserId
				from Procurement.RFQVendorResponse vr
				INNER JOIN Procurement.Vendors v ON vr.VendorCode = v.VendorCode
				INNER JOIN Config.AppUser au ON v.VendorId = au.VendorId
				INNER JOIN Procurement.RFQMain rm ON vr.RFQMainId = rm.RFQMainId
				where vr.MailIsSentDeadLine = 0 and
				rm.Status = 1 and
				rm.IsDeleted != 1 and
				CONVERT(DATE , rm.RFQDeadline) = CONVERT(DATE, DATEADD(DAY, +2, GETDATE()))
				";

			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = text;

				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					userDatas.Add(new RFQUserData
					{
						RFQVendorResponseId = reader.Get<int>("RFQVendorResponseId"),
						RFQMainId = reader.Get<int>("RFQMainId"),
						Email = reader.Get<string>("Email"),
						Language = reader.Get<string>("Language"),
						VendorCode = reader.Get<string>("VendorCode"),
						VendorName = reader.Get<string>("VendorName"),
						UserId = reader.Get<int>("UserId")
					});
				}
				return userDatas;
			}
		}
		
		public async Task<List<RFQUserData>> GetRFQVendorUsersMailIsSentLastDayFalse()
		{
			List<RFQUserData> userDatas = new();

			var text = @"
				select 
    			vr.RFQVendorResponseId,
    			vr.RFQMainId,
    			au.Language,
			    vr.VendorCode,
			    v.VendorName,
			    au.Email ,
			    au.Id as UserId
				from Procurement.RFQVendorResponse vr
				INNER JOIN Procurement.Vendors v ON vr.VendorCode = v.VendorCode 
				INNER JOIN Config.AppUser au ON v.VendorId = au.VendorId 
				INNER JOIN Procurement.RFQMain rm ON vr.RFQMainId = rm.RFQMainId 
				where vr.MailIsSentLastDay = 0 and
				rm.Status = 1 and
				rm.IsDeleted != 1 and
				CONVERT(DATE , rm.RFQDeadline) = CONVERT(DATE, DATEADD(DAY, +1, GETDATE()))
				";
			
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = text;

				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					userDatas.Add(new RFQUserData
					{
						RFQVendorResponseId = reader.Get<int>("RFQVendorResponseId"),
						RFQMainId = reader.Get<int>("RFQMainId"),
						Email = reader.Get<string>("Email"),
						Language = reader.Get<string>("Language"),
						VendorCode = reader.Get<string>("VendorCode"),
						VendorName = reader.Get<string>("VendorName"),
						UserId = reader.Get<int>("UserId")
					});
				}
				return userDatas;
			}
		}

		public async Task<bool> UpdateIsSent(List<int> rfqVendorResponseIds)
		{
			var idsRes = string.Join(",", rfqVendorResponseIds);
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = @$"set nocount off update Procurement.RFQVendorResponse set MailIsSent = 1 where RFQVendorResponseId IN({idsRes})";
				try
				{
					var res = command.ExecuteNonQuery() > 0;
					return res;
				}
				catch (Exception ex)
				{
					return false;
				}


			}
		}
		
		public async Task UpdateMailIsSentDeadLine(List<int> rfqVendorResponseIds)
		{
			var idsRes = rfqVendorResponseIds.ToArray();
			
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = @"SET NOCOUNT OFF EXEC SP_RFQVendorResponseUpdateDeadLine @RFQVendorResponseId";

				string ids = string.Join(",", idsRes); // Assuming you can pass a CSV of values to the query
				command.Parameters.AddWithValue(command, "@RFQVendorResponseId", ids);
				Console.WriteLine("UpdateMailIsSentDeadLine "+ids);
				await _unitOfWork.SaveChangesAsync();
				await command.ExecuteNonQueryAsync();
			}

		}
		
		public async Task UpdateMailIsSentLastDay(List<int> rfqVendorResponseIds)
		{
			var idsRes = rfqVendorResponseIds.ToArray();

			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = @"SET NOCOUNT OFF EXEC SP_RFQVendorResponseUpdateLastDay @RFQVendorResponseId";

				string ids = string.Join(",", idsRes); // Assuming you can pass a CSV of values to the query
				Console.WriteLine("UpdateMailIsSentLastDay "+ids);
				command.Parameters.AddWithValue(command, "@RFQVendorResponseId", ids);
				await _unitOfWork.SaveChangesAsync();
				await command.ExecuteNonQueryAsync();
			}
		}

	}
}
