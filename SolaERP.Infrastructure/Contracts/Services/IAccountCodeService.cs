using SolaERP.Application.Dtos.Account;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAccountCodeService : ICrudService<AccountCodeDto>
    {
        Task<ApiResponse<List<AccountCodeDto>>> GetAccountCodesByBusinessUnit(string businessUnitCode);
    }
}
