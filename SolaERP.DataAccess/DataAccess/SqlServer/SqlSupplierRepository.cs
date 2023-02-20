using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Supplier;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlSupplierRepository : ISupplierRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlSupplierRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SupplierCode>> GetSupplierCodesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_Supplier_List";
                using var reader = await command.ExecuteReaderAsync();

                List<SupplierCode> result = new();
                while (await reader.ReadAsync()) result.Add(reader.GetByEntityStructure<SupplierCode>());

                return result;
            }
        }
    }
}
