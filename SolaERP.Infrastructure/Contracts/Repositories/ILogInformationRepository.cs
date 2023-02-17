using SolaERP.Infrastructure.Entities.LogInfo;
using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ILogInformationRepository
    {
        public Task<List<LogInfo>> GetAllLogInformationAsync(int id, LogType logType);
    }
}
