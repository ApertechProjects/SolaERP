using SolaERP.Application.Entities.AccountCode;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAccountCodeRepository : ICrudOperations<AccountCode>
    {
        Task<List<AccountCode>> GetAccountCodesByBusinessUnit(int businessUnit);

		Task<List<AccountCode>> GetAccountCodesByBusinessUnitAndAnlCode(int businessUnit, string anlCode);

	}
}
