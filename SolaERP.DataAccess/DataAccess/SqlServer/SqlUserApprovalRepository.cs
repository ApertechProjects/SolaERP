using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlUserApprovalRepository : IUserApprovalRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SqlUserApprovalRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CheckIsUserApproved(int userId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"SELECT TOP 1 ApproveStatus FROM Config.UserApproval UA 
                                WHERE Id = @UserId ORDER BY Sequence DESC";
        command.Parameters.AddWithValue(command, "@UserId", userId);
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return true;
        var approveStatus = reader.Get<int>("ApproveStatus");
        return approveStatus == 1;
    }
}