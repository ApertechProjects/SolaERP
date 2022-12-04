using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IApproveStageMainService : ICrudService<ApproveStagesMainDto>
    {
        Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId);
    }
}
