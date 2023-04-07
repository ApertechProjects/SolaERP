using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
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

        public async Task<bool> DeleteBuyerByGroupIdAsync(int groupBuyerId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupBuyers_IUD @GroupBuyerId";
                command.Parameters.AddWithValue(command, "@GroupBuyerId", groupBuyerId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<Buyer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Buyer>> GetBuyerByUserTokenAsync(int userId, string businessUnitCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[SP_UNI_Buyer_List]", new ReplaceParams { ParamName = "APT", Value = businessUnitCode });
                using var reader = await command.ExecuteReaderAsync();

                List<Buyer> status = new List<Buyer>();

                while (reader.Read())
                {
                    status.Add(reader.GetByEntityStructure<Buyer>());
                }
                return status;
            }
        }

        public async Task<List<GroupBuyer>> GetBuyersByGroupIdAsync(int groupId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_GroupBuyers_Load @GroupId";
                command.Parameters.AddWithValue(command, "@GroupId", groupId);
                using var reader = await command.ExecuteReaderAsync();

                List<GroupBuyer> buyers = new List<GroupBuyer>();

                while (reader.Read())
                {
                    buyers.Add(reader.GetByEntityStructure<GroupBuyer>());
                }
                return buyers;
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

        public async Task<bool> SaveBuyerByGroupAsync(GroupBuyerSaveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF Exec SP_GroupBuyers_IUD  @GroupBuyerId,
                                                                       @GroupId,  
                                                                       @BusinessUnitId, 
                                                                       @BuyerCode";

                command.Parameters.AddWithValue(command, "@GroupBuyerId", model.GroupBuyerId);
                command.Parameters.AddWithValue(command, "@GroupId", model.GroupId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@BuyerCode", model.BuyerCode);


                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public Task UpdateAsync(Buyer entity)
        {
            throw new NotImplementedException();
        }
    }
}
