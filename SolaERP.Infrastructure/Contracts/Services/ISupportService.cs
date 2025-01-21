using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Support;
using SolaERP.Application.Dtos.UOM;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;


namespace SolaERP.Application.Contracts.Services
{
	public interface ISupportService
	{
		Task<ApiResponse<int>> Save(SupportSaveDto dto, string userIdentity);
	}
}
