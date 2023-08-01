using SolaERP.Application.Entities.General;
using SolaERP.Application.Entities.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGeneralRepository
    {
        Task<List<Status>> GetStatus();
        Task<List<RejectReason>> RejectReasons();

    }
}
