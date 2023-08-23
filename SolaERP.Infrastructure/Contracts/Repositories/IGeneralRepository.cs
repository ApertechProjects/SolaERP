using SolaERP.Application.Entities.General;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Entities.SupplierEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGeneralRepository
    {
        Task<List<BusinessCategory>> BusinessCategories();
        Task<List<Status>> GetStatus();
        Task<List<RejectReason>> RejectReasons();

    }
}
