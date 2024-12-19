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

	}
}
