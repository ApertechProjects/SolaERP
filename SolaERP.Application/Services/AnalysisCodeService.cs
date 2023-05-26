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
        private readonly IAnalysisStructureRepository _analysisCodeRepository;
        private readonly IUserRepository _userRepository;

        public AnalysisCodeService(IMapper mapper, IUnitOfWork unitOfWork, IAnalysisStructureRepository analysisCodeRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _analysisCodeRepository = analysisCodeRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(AnalysisCodeDeleteModel model, string userName)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(userName);
            var code = false;
            int counter = 0;
            for (int i = 0; i < model.AnalysisCodeIds.Count; i++)
            {
                code = await _analysisCodeRepository.DeleteAnalysisCodeAsync(model.AnalysisCodeIds[i], userId);
                if (code)
                    counter++;
            }

            if (counter == model.AnalysisCodeIds.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(code, 200);
            }
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

        public async Task<ApiResponse<List<AnalysisWithBuDto>>> GetByBUIdAsync(int businessUnitId, string userName)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(userName);
            var data = await _analysisCodeRepository.GetByBUIdAsync(businessUnitId, userId);
            var map = _mapper.Map<List<AnalysisWithBuDto>>(data);
            if (map.Count > 0)
                return ApiResponse<List<AnalysisWithBuDto>>.Success(map, 200);
            return ApiResponse<List<AnalysisWithBuDto>>.Fail("Analysis Codes is empty", 400);
        }

        public async Task<ApiResponse<List<AnalysisCodesDto>>> GetByDimensionIdAsync(int dimensionId)
        {
            var data = await _analysisCodeRepository.GetAnalysisCodesByDimensionIdAsync(dimensionId);
            var map = _mapper.Map<List<AnalysisCodesDto>>(data);
            if (map.Count > 0)
                return ApiResponse<List<AnalysisCodesDto>>.Success(map, 200);
            return ApiResponse<List<AnalysisCodesDto>>.Fail("Analysis Codes is empty", 400);

        }

        public async Task<ApiResponse<bool>> SaveAnalysisCodeAsync(List<AnalysisCodeSaveModel> analysisCodeSave, string name)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var code = false;
            int counter = 0;
            for (int i = 0; i < analysisCodeSave.Count; i++)
            {
                code = await _analysisCodeRepository.SaveAnalysisCode(analysisCodeSave[i], userId);
                if (code)
                    counter++;
            }

            if (counter == analysisCodeSave.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(code, 200);
            }

            return ApiResponse<bool>.Fail("Analysis code can not be saved", 400);
        }
    }
}
