using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.UOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IUOMRepository : ICrudOperations<UOM>
    {
        Task<List<UOM>> GetUOMListBusinessUnitCode(string businessUnitCode);
    }
}
