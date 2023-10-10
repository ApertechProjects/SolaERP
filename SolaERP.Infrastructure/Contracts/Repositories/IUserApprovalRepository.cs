namespace SolaERP.Application.Contracts.Repositories;

public interface IUserApprovalRepository
{
    Task<bool> CheckIsUserApproved(int userId);
}