using SolaERP.Infrastructure.Entities.Log;
using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ILogInformationRepository
    {
        public Task<LogInfo> GetSingleLogInformationAsync(int id, LogType logType);
        public Task<List<LogInfo>> GetAllLogInformationAsync(int id, LogType logType);
    }
}
