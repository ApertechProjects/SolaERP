using AutoMapper;
using MediatR;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.AnalysisDimension;

namespace SolaERP.Infrastructure.Features.Queries.AnalysisCode
{
    public class GetAnalysisDimensionByBuRequestHandler : IRequestHandler<GetAnalysisDimensionByBuRequest, ApiResponse<List<GetAnalysisDimensionByBuResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAnalysisCodeRepository _repository;

        public GetAnalysisDimensionByBuRequestHandler(IAnalysisCodeRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ApiResponse<List<GetAnalysisDimensionByBuResponse>>> Handle(GetAnalysisDimensionByBuRequest request, CancellationToken cancellationToken)
        {
            List<BuAnalysisDimension> dimensions = await _repository.GetBusinessUnitDimensions(request.BusinessunitId);

            if (dimensions == null || dimensions.Count == 0)
            {
                dimensions = new();
                return ApiResponse<List<GetAnalysisDimensionByBuResponse>>.Fail("Analysis Dimension not found for the given Business Unit ID", 404);
            }

            List<GetAnalysisDimensionByBuResponse> response = _mapper.Map<List<GetAnalysisDimensionByBuResponse>>(dimensions);
            return ApiResponse<List<GetAnalysisDimensionByBuResponse>>.Success(response, 200);
        }
    }
}
