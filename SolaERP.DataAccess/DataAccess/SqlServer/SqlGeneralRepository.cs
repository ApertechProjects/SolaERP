using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.General;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGeneralRepository : IGeneralRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlGeneralRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Status>> GetStatus()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_Status_List";
                using var reader = await command.ExecuteReaderAsync();

                List<Status> status = new List<Status>();

                while (reader.Read())
                {
                    status.Add(reader.GetByEntityStructure<Status>());
                }
                return status;
            }
        }

        public async Task<List<RejectReason>> RejectReasons()
        {
            List<RejectReason> rejectReasons = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from VW_RejectReasons";
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    rejectReasons.Add(reader.GetByEntityStructure<RejectReason>());
                }
                return rejectReasons;
            }
        }
    }
}
