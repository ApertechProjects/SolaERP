using SolaERP.Application.Entities.LogInfo;
using SolaERP.Application.Enums;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ILogInformationRepository
    {
        public Task<List<LogInfo>> GetAllLogInformationAsync(int id, LogType logType);
    }
}
