using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IGeneralService
    {
        Task<ApiResponse<List<RejectReasonDto>>> RejectReasons();
    }
}
