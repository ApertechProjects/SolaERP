using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Entities.Location;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task UpdateAsync(Location entity)
        {
            throw new NotImplementedException();
        }
    }
}
