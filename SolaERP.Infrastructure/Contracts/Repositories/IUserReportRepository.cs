using SolaERP.Application.Dtos.UserReport;
using SolaERP.Application.Entities.UserReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IUserReportRepository
    {
        Task<bool> Delete(string reportFileId);
		Task<string> GetFileNameByReportFileId(int id);
		Task<bool> Save(UserReportFileAccess data);
        Task<int> SaveTest(UserReportFileAccess data);
    }
}
