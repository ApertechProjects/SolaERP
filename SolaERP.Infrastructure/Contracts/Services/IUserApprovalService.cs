namespace SolaERP.Application.Contracts.Services;

public interface IUserApprovalService
{
    Task<bool> CheckIsUserApproved(int userId);
}