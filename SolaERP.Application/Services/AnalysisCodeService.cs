using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class AnalysisCodeService : IAnalysisCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAnalysisCodeRepository _analysisCodeRepository;
        private readonly IUserRepository _userRepository;

        public AnalysisCodeService(IMapper mapper, IUnitOfWork unitOfWork, IAnalysisCodeRepository analysisCodeRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _analysisCodeRepository = analysisCodeRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(int analysisCodeId)
        {
            var code = await _analysisCodeRepository.DeleteAnalysisCodeAsync(analysisCodeId);
            await _unitOfWork.SaveChangesAsync();
            if (code)
                return ApiResponse<bool>.Success(code, 200);
            return ApiResponse<bool>.Fail("Analysis code can not be deleted", 400);
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

        public async Task<ApiResponse<List<AnalysisDto>>> GetAnalysisCodesAsync(int analysisCodeId, string userName)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(userName);
            var data = await _analysisCodeRepository.GetAnalysisCodesAsync(analysisCodeId, userId);
            var map = _mapper.Map<List<AnalysisDto>>(data);
            if (map.Count > 0)
                return ApiResponse<List<AnalysisDto>>.Success(map, 200);
            return ApiResponse<List<AnalysisDto>>.Fail("Analysis Codes is empty", 400);
        }

        public async Task<ApiResponse<List<AnalysisCodesDto>>> GetAnalysisCodesByDimensionIdAsync(int dimensionId)
        {
            var data = await _analysisCodeRepository.GetAnalysisCodesByDimensionIdAsync(dimensionId);
            var map = _mapper.Map<List<AnalysisCodesDto>>(data);
            if (map.Count > 0)
                return ApiResponse<List<AnalysisCodesDto>>.Success(map, 200);
            return ApiResponse<List<AnalysisCodesDto>>.Fail("Analysis Codes is empty", 400);

        }

        public async Task<ApiResponse<List<AnalysisDimensionDto>>> GetAnalysisDimensionAsync()
        {
            var codes = await _analysisCodeRepository.GetAnalysisDimensionAsync();
            var dto = _mapper.Map<List<AnalysisDimensionDto>>(codes);
            return ApiResponse<List<AnalysisDimensionDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> SaveAnalysisCodeAsync(AnalysisDto analysisDto, string name)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var code = await _analysisCodeRepository.SaveAnalysisCode(analysisDto, userId);
            await _unitOfWork.SaveChangesAsync();
            if (code)
                return ApiResponse<bool>.Success(code, 200);
            return ApiResponse<bool>.Fail("Analysis code can not be saved", 400);
        }
    }
}
