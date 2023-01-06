using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlRequestDetailRepository : IRequestDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlRequestDetailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(RequestDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<RequestDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RequestDetail> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(RequestDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
