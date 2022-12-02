using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBusinessUnitRepository : IBusinessUnitRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBusinessUnitRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(BusinessUnits entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<BusinessUnits>> GetAllAsync()
        {
            var result = Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = "Select * from dbo.VW_BusinessUnits_List";
                    using var reader = command.ExecuteReader();

                    List<BusinessUnits> businessUnits = new List<BusinessUnits>();

                    while (reader.Read())
                    {
                        businessUnits.Add(reader.GetByEntityStructure<BusinessUnits>());
                    }
                    return businessUnits;
                }
            });
            return result;
        }

        public Task<List<BusinessUnits>> GetBusinessUnitListByUserId(int userId)
        {
            var result = Task.Run(() =>
            {
                using (var command = _unitOfWork.CreateCommand())
                {
                    command.CommandText = $"EXEC SP_BusinessUnitsList {userId}";
                    using var reader = command.ExecuteReader();

                    List<BusinessUnits> businessUnits = new List<BusinessUnits>();

                    while (reader.Read())
                    {
                        businessUnits.Add(reader.GetByEntityStructure<BusinessUnits>());
                    }
                    return businessUnits;
                }
            });
            return result;
        }

        public Task<BusinessUnits> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(BusinessUnits entity)
        {
            throw new NotImplementedException();
        }
    }
}
