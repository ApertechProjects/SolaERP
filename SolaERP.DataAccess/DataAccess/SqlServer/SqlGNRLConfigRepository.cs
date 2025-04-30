using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;
using System.Data.SqlClient;
using SolaERP.Application.Entities;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGNRLConfigRepository : SqlBaseRepository, IGNRLConfigRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlGNRLConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GNRLConfig>> GetGNRLConfigsByBusinessUnitId(int businessUnitId)
        {
            var configs = new List<GNRLConfig>();
            await using var command = _unitOfWork.CreateCommand() as SqlCommand;
            command.CommandText = "EXEC dbo.SP_GRNLConfigLoad @BusinessUnitId";

            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                configs.Add(reader.GetByEntityStructure<GNRLConfig>());
            }
            return configs;
        }
    }
}
