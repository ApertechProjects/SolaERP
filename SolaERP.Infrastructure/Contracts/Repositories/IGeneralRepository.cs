using SolaERP.Application.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGeneralRepository
    {
        Task<List<RejectReason>> RejectReasons();

    }
}
