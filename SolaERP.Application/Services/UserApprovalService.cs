using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Persistence.Services;

public class UserApprovalService : IUserApprovalService
{
    private readonly IUserApprovalRepository _userApprovalRepository;

    public UserApprovalService(IUserApprovalRepository userApprovalRepository)
    {
        _userApprovalRepository = userApprovalRepository;
    }

    public async Task<bool> CheckIsUserApproved(int userId)
    {
        return await _userApprovalRepository.CheckIsUserApproved(userId);
    }
}