using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Location;
using SolaERP.Application.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlLocationRepository : ILocationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlLocationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(Location entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Location>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select * from dbo.VW_UNI_LocationList";
                using var reader = await command.ExecuteReaderAsync();

                List<Location> locations = new List<Location>();

                while (reader.Read())
                {
                    locations.Add(reader.GetByEntityStructure<Location>());
                }
                return locations;
            }
        }

        public Task<Location> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Location>> GetAllByBusinessUnitId(int businessUnitId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "exec SP_LocationList @BusinessUnitId";
            command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);
            await using var reader = await command.ExecuteReaderAsync();

            var locations = new List<Location>();

            while (await reader.ReadAsync())
            {
                locations.Add(reader.GetByEntityStructure<Location>());
            }
            return locations;
        }

        public Task UpdateAsync(Location entity)
        {
            throw new NotImplementedException();
        }
    }
}
