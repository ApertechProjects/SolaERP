using MediatR;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Features.Queries.AnalysisCode
{
    public class GetAnalysisDimensionByBuRequest : IRequest<ApiResponse<List<GetAnalysisDimensionByBuResponse>>>
    {
        public int BusinessunitId { get; set; }
    }
}
