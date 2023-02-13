using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlItemCodeRepository : IItemCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlItemCodeRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<ItemCode>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_UNI_ItemList";
                List<ItemCode> result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result.Add(GetItemCodeFromReader(reader));

                return result;
            }
        }

        public async Task<ItemCode> GetItemCodeByItemCodeAsync(string itemCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM [dbo].[GET_ITEM_BY_ITEM_CODE](@ItemCode)";
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);
                ItemCode result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result = reader.GetByEntityStructure<ItemCode>();

                return result;
            }
        }

        public async Task<List<ItemCodeWithImages>> GetItemCodesWithImagesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_UNI_ItemImagesList";
                List<ItemCodeWithImages> result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<ItemCodeWithImages>());

                return result;
            }
        }

        private ItemCode GetItemCodeFromReader(IDataReader reader)
        {
            return new()
            {
                Item_Code = reader.Get<string>("ItemCode").Trim(),
                Description = reader.Get<string>("Description"),
                LongDescription = reader.Get<string>("LongDescription"),
                UnitOfPurch = reader.Get<string>("UnitOfPurch")
            };
        }

    }
}
