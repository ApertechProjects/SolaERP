using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageDetailService : ICrudService<ApproveStagesDetailDto>
    {
        Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId);
    }
}
