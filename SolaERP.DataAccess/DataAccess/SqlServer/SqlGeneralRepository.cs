using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.General;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGeneralRepository : IGeneralRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public Task<List<RejectReason>> RejectReasons()
        {
            throw new NotImplementedException();
        }
    }
}
