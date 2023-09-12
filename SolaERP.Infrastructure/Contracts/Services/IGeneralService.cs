using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Entities.SupplierEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IGeneralService
    {
        Task<ApiResponse<List<BusinessCategory>>> BusinessCategories();
        Task<ApiResponse<List<StatusDto>>> GetStatus();
        Task<ApiResponse<List<RejectReasonDto>>> RejectReasons();
        Task<ApiResponse<BaseAndReportCurrencyRate>> GetBaseAndReportCurrencyRateAsync(
            DateTime date,
            string currency, 
            int businessUnitId);
    }
}