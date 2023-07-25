using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IRfqService
    {
        Task<ApiResponse<List<RfqDraftDto>>> GetDraftsAsync(RfqFilter filter);
    }
}
