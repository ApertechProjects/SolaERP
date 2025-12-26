using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Entitlement;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services;

public class EntitlementService : IEntitlementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEntitlementRepository _entitlementRepository;

    public EntitlementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEntitlementRepository entitlementRepository
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _entitlementRepository = entitlementRepository;
    }

    public async Task<ApiResponse<bool>> SaveEntitlementsAsync(List<EntitlementUIDDto> data)
    {
        await _entitlementRepository.SaveEntitlementIUD(data);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true, 200);
    }

    public async Task<ApiResponse<List<EntitlementListDto>>> GetEntitlementListAsync(int businessUnitId, int periodFrom,
        int periodTo)
    {
        var result = await _entitlementRepository.GetEntitlementsList(businessUnitId, periodFrom, periodTo);
        return ApiResponse<List<EntitlementListDto>>.Success(result, 200);
    }
}