using SolaERP.Application.Entities.Auth;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using SolaERP.Job.RFQClose;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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

        public async Task<List<RFQUserData>> GetRFQVendorUsers(int rfqMainId)
        {
            List<RFQUserData> userDatas = new();

            var text = "select au.Language,vr.VendorCode,au.FullName,au.UserName from Procurement.RFQVendorResponse vr\r\nINNER JOIN Procurement.Vendors v\r\nON vr.VendorCode = v.VendorCode\r\nINNER JOIN Config.AppUser au\r\nON v.VendorId = au.VendorId\r\nwhere RFQMainId = @rfqMainId";

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = text;
                command.Parameters.AddWithValue(command, "@rfqMainId", rfqMainId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userDatas.Add(new RFQUserData
                    {
                        Language = reader.Get<string>("Language"),
                        VendorCode = reader.Get<string>("VendorCode"),
                        FullName = reader.Get<string>("FullName"),
                        UserName = reader.Get<string>("UserName")
                    });
                }
                return userDatas;
            }
        }

        public List<int> GetClosedRFQs()
        {
            return new List<int> { 1, 2, 3, };
        }
    }
}
