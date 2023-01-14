namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IRequestMainRepository : IDeleteableAsync, IReturnableRepoMethodAsync<RequestMain>
    {
        Task<bool> ChangeRequestStatusAsync(RequestChangeStatusParametersDto requestChangeStatusParametersDto);
        Task<List<RequestMain>> GetAllAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo, ApproveStatuses ApproveStatus, Status Status);
        Task<List<RequestTypes>> GetRequestTypesByBusinessUnitIdAsync(int businessUnitId);
        Task<List<RequestMainDraft>> GetAllMainRequestDraftsAsync(int businessUnitId, string itemCode, DateTime dateFrom, DateTime dateTo);
        Task<bool> SendRequestToApproveAsync(int userId, int requestMainId);
        Task<List<RequestMain>> GetApproveAmendmentRequests(int userId, RequestApproveAmendmentGetParametersDto requestParametersDto);
        Task<List<RequestTypes>> GetRequestTypesByBusinessUnitId(int businessUnitId);
    }
}
