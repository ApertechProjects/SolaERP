using SolaERP.Application.Entities.General;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Dtos.General;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGeneralRepository
    {
        Task<List<BusinessCategory>> BusinessCategories();
        Task<List<Status>> GetStatus();
        Task<List<RejectReason>> RejectReasons();
        Task<List<ConvRateDto>> GetConvRateList(int businessUnitId);

    }
}
