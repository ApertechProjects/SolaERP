using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Support;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
	public interface ISupportRepository
	{
		Task<int> Save(SupportSaveDto dto);
	}
}
