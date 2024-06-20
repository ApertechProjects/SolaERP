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
        Task<bool> Save(UserReportFileAccess data);
    }
}
