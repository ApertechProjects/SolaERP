using MediatR;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Features.Queries.AnalysisCode
{
    public class GetAnalysisDimensionByBuRequest : IRequest<ApiResponse<List<GetAnalysisDimensionByBuResponse>>>
    {
        public int BusinessunitId { get; set; }
    }
}
