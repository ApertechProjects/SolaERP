using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Bid;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IBidService
    {
        Task<ApiResponse<List<BidAllDto>>> GetAllAsync(BidAllFilterDto filter);
        Task<ApiResponse<List<BidDetailsLoadDto>>> GetBidDetailsAsync(BidDetailsFilterDto filter);
        Task<ApiResponse<BidMainLoadDto>> GetMainLoadAsync(int bidMainId);
        Task<ApiResponse<BidCardDto>> GetBidCardAsync(int bidMainId);
        Task<ApiResponse<BidIUDResponse>> SaveBidMainAsync(BidMainDto bidMain, string userIdentity);
        Task<ApiResponse<bool>> DeleteBidMainAsync(int bidMainId, string userIdentity);
        Task<ApiResponse<bool>> BidDisqualifyAsync(BidDisqualifyDto dto, string userIdentity);
        Task<ApiResponse<List<BidRFQListLoadDto>>> GetRfqListAsync(string userIdentity);

    }
}
