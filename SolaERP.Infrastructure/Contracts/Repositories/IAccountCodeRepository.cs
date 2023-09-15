using SolaERP.Application.Entities.AccountCode;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAccountCodeRepository : ICrudOperations<AccountCode>
    {
        Task<List<AccountCode>> GetAccountCodesByBusinessUnit(int businessUnit);
    }
}
