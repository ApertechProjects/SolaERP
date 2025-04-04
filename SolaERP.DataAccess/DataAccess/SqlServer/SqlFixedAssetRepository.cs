using System.Data.Common;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.FixedAsset;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;

namespace SolaERP.DataAccess.DataAccess.SqlServer;

public class SqlFixedAssetRepository : IFixedAssetRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SqlFixedAssetRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FixedAsset>> GetAllAsync(int businessUnitId)
    {
        await using var command = _unitOfWork.CreateCommand() as DbCommand;
        command.CommandText = @"EXEC SP_FixedAssetList @BusinessUnitId";

        command.Parameters.AddWithValue(command, "@BusinessUnitId", businessUnitId);

        await using var reader = await command.ExecuteReaderAsync();
        List<FixedAsset> data = new();
        while (await reader.ReadAsync())
            data.Add(new FixedAsset
            {
                AssetCode = reader.Get<string>("AssetCode"),
                Description = reader.Get<string>("Description")
            });
        return data;
    }
}