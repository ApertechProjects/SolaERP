using SolaERP.Infrastructure.Dtos.Account;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IAccountCodeService : ICrudService<AccountCodeDto>
    {
        Task<ApiResponse<List<AccountCodeDto>>> GetAccountCodesByBusinessUnit(string businessUnit);
    }
}
