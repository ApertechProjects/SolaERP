using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using SolaERP.Application.Dtos.Buyer;

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
                command.CommandText = "exec [dbo].[SP_UNI_Buyer_List] @UserId,@BusinessUnitId";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
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

        public async Task<string> FindBuyerEmailByBuyerName(string buyerName, int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_UNI_Buyer_List_FOREMAIL @BusinessUnitId, @BuyerName";
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@BuyerName", buyerName);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            string result = null;
            if (await reader.ReadAsync())
            {
                result = reader.Get<string>("BuyerEmail");
            }

            return result;
        }

        public async Task<BuyerDto> FindBuyerDataByBuyerName(string buyerName, int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"exec dbo.SP_UNI_Buyer_List_FOREMAIL @BusinessUnitId, @BuyerName";
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            command.Parameters.AddWithValue(command, "@BuyerName", buyerName);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            BuyerDto data = new BuyerDto();
            if (await reader.ReadAsync())
            {
                data = new BuyerDto()
                {
                    BuyerEmail = reader.Get<string>("BuyerEmail"),
                    BuyerName = reader.Get<string>("BuyerName"),
                    BuyerCode = reader.Get<string>("BuyerCode"),
                    BusinessUnitName = reader.Get<string>("BusinessUnitName"),
                    BusinessUnitCode = reader.Get<string>("BusinessUnitCode"),
                    BusinessUnitId = reader.Get<int>("BusinessUnitId")
                };
            }

            return data;
        }
    }
}