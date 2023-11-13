using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Entities.Request;
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
    public class SqlBuyerRepository : SqlBaseRepository, IBuyerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBuyerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Buyer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Buyer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Buyer>> GetBuyersAsync(int userId, int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec [dbo].[SP_UNI_Buyer_List] @userId,@businessUnitId";
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@userId", userId);
                using var reader = await command.ExecuteReaderAsync();
                List<Buyer> status = new List<Buyer>();

                while (reader.Read())
                {
                    status.Add(reader.GetByEntityStructure<Buyer>());
                }
                return status;
            }
        }

        public Task<Buyer> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Buyer entity)
        {
            throw new NotImplementedException();
        }
    }
}
