using SolaERP.Infrastructure.Entities.ApproveStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IApproveStageMainRepository:ICrudOperations<ApproveStagesMain>
    {
        Task<List<ApproveStagesMain>> GetByBusinessUnitId(int buId);
    }
}
