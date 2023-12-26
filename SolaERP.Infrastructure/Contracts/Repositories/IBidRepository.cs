using SolaERP.Application.Entities.Bid;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBidRepository
    {
        Task<List<BidAll>> GetAllAsync(BidAllFilter filter);
        Task<List<BidDetailsLoad>> GetBidDetailsAsync(BidDetailsFilter filter);
        Task<BidMainLoad> GetMainLoadAsync(int bidMainId);
        Task<BidIUDResponse> BidMainIUDAsync(BidMain entity);
        Task<bool> BidDisqualifyAsync(BidDisqualify entity);
        Task<bool> SaveBidDetailsAsync(List<BidDetail> details);
        Task<List<BidRFQListLoad>> GetRFQListForBidAsync(BidRFQListFilter filter);
        Task OrderCreateFromApproveBidAsync(int bidMainId, int userId);
        Task<List<int>> GetDetailIds(int id);
    }
}
