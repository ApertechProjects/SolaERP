using SolaERP.Application.Dtos.Account;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAccountCodeService : ICrudService<AccountCodeDto>
    {
        Task<ApiResponse<List<AccountCodeDto>>> GetAccountCodesByBusinessUnit(int businesUnitId);
		Task<ApiResponse<List<AccountCodeDto>>> GetAccountCodesByBusinessUnitAndAnlCode(int businesUnitId, string anlCode);
		
	}
}
