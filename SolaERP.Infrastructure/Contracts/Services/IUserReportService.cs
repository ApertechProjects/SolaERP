using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UserReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IUserReportService
    {
        Task<ApiResponse<bool>> Save(UserReportSaveDto data);
    }
}
