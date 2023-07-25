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

    }
}
