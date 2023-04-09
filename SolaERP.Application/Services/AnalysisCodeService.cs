using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.AnaysisDimension;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class AnalysisCodeService : IAnalysisCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAnalysisCodeRepository _analysisCodeRepository;

        public AnalysisCodeService(IMapper mapper, IUnitOfWork unitOfWork, IAnalysisCodeRepository analysisCodeRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _analysisCodeRepository = analysisCodeRepository;
        }

        public async Task<ApiResponse<List<IGrouping<int, AnalysisCodeDto>>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest)
        {
            var analysisCodes = await _analysisCodeRepository.GetAnalysisCodesAsync(getRequest.Businessunitid, getRequest.ProcedureName);
            List<AnalysisCodeDto> analysis = _mapper.Map<List<AnalysisCodeDto>>(analysisCodes);

            var groupingReult = analysis.GroupBy(x => x.Sequence).ToList();


            var analysisCodeResult = _mapper.Map<List<AnalysisCodeDto>>(analysisCodes);

            return analysisCodeResult.Count > 0 ? ApiResponse<List<IGrouping<int, AnalysisCodeDto>>>.Success(groupingReult, 200) :
                  ApiResponse<List<IGrouping<int, AnalysisCodeDto>>>.Fail("analysisCodes", "Analysis codes is empty", 404, true);
        }


        public async Task<ApiResponse<List<AnalysisDimensionDto>>> GetAnalysisDimensionAsync()
        {
            var codes = await _analysisCodeRepository.GetAnalysisDimensionAsync();
            var dto = _mapper.Map<List<AnalysisDimensionDto>>(codes);
            return ApiResponse<List<AnalysisDimensionDto>>.Success(dto, 200);
        }

    }
}
