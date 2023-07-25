using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IRfqRepository
    {
        Task<List<RfqDraft>> GetDraftsAsync(RfqFilter filter);
        Task<List<RfqAll>> GetAllAsync(RfqAllFilter filter);
        Task<List<SingleSourceReasonModel>> GetSingleSourceReasonsAsync();

        Task<int> AddMainAsync(RfqSaveCommandRequest request);
        Task<int> UpdateMainAsync(RfqSaveCommandRequest request);
        Task<int> DeleteMainsync(int id, int userId);

    }
}
