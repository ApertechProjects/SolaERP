using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBusinessUnitRepository : IBusinessUnitRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBusinessUnitRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Add(BusinessUnits entity)
        {
            throw new NotImplementedException();
        }

        public List<BusinessUnits> GetAllAsync()
        {
            using(var command = _unitOfWork.CreateCommand())
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
        }

        public Task<BusinessUnits> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(BusinessUnits entity)
        {
            throw new NotImplementedException();
        }

        public void Update(BusinessUnits entity)
        {
            throw new NotImplementedException();
        }
    }
}
