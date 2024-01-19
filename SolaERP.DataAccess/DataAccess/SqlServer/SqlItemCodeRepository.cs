using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Item_Code;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
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


        public async Task<List<ItemCode>> GetAllAsync(int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_ItemList @BusinessUnitId";
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
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
                command.CommandText = ReplaceQuery("[dbo].[GET_ITEM_BY_ITEM_CODE]",
                    new ReplaceParams { ParamName = "APT", Value = businessUnitCode });
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);
                ItemCodeWithImages result = new();

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result = reader.GetByEntityStructure<ItemCodeWithImages>();

                return result;
            }
        }

        public async Task<ItemCodeInfo> GetItemCodeInfoByItemCodeAsync(string itemCode, int businessUnitId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_ItemInfo @ItemCode,@BusinessUnitId";
                command.Parameters.AddWithValue(command, "@ItemCode", itemCode);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
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
                UnitOfPurch = reader.Get<string>("UnitOfPurch"),
                ItemDescriptionAze = reader.Get<string>("ItemDescriptionAze"),
                AccountCode = reader.Get<string>("AccountCode"),
                AccountName = reader.Get<string>("AccountName")
            };
        }
    }
}