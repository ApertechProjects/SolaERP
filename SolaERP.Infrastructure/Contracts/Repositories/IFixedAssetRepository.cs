using SolaERP.Application.Entities.FixedAsset;

namespace SolaERP.Application.Contracts.Repositories;

public interface IFixedAssetRepository
{
    Task<List<FixedAsset>> GetAllAsync(int detailId);
}