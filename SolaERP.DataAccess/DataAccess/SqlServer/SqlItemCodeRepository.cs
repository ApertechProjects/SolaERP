using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Dtos.Item_Code;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlItemCodeRepository : SqlBaseRepository, IItemCodeRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlItemCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
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
                while (await reader.ReadAsync())
                    result.Add(GetItemCodeFromReader(reader));

                return result;
            }
        }

        public async Task<List<ItemCode>> GetAllAsync(string businessUnit)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[VW_UNI_ItemList]", new ReplaceParams { ParamName = "APT", Value = businessUnit });
                List<ItemCode> result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    result.Add(GetItemCodeFromReader(reader));

                return result;
            }
        }


        public async Task<ItemCodeWithImages> GetItemCodeByItemCodeAsync(string businessUnitCode, string itemCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = ReplaceQuery("[dbo].[GET_ITEM_BY_ITEM_CODE](@ItemCodes)", new ReplaceParams { ParamName = "APT", Value = businessUnitCode });
                command.Parameters.AddWithValue(command, "@ItemCodes", itemCode);
                ItemCodeWithImages result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result = reader.GetByEntityStructure<ItemCodeWithImages>();

                return result;
            }
        }

        public async Task<ItemCodeInfo> GetItemCodeInfoByItemCodeAsync(string itemCode)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_ItemInfo @ItemCode";
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);
                ItemCodeInfo result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result = reader.GetByEntityStructure<ItemCodeInfo>();

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
