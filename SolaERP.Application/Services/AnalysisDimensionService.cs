using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class AnalysisDimensionService : IAnalysisDimensionService
    {
        private readonly IAnalysisDimensionRepository _analysisDimensionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AnalysisDimensionService(IAnalysisDimensionRepository analysisDimensionRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _analysisDimensionRepository = analysisDimensionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AnalysisDimensionDto>>> ByAnalysisDimensionId(int analysisDimensionId, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _analysisDimensionRepository.ByAnalysisDimensionId(analysisDimensionId, userId);
            var dto = _mapper.Map<List<AnalysisDimensionDto>>(data);
            return ApiResponse<List<AnalysisDimensionDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<BuAnalysisDimensionDto>>> ByBusinessUnitId(int businessUnitId, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _analysisDimensionRepository.ByBusinessUnitId(businessUnitId, userId);
            var dto = _mapper.Map<List<BuAnalysisDimensionDto>>(data);
            return ApiResponse<List<BuAnalysisDimensionDto>>.Success(dto, 200);
        }

        public async Task<List<DimensionCheckDto>> CheckDimensionIdIsUsedInStructure(List<int> dimensionIds)
        {
            var existDatas = await _analysisDimensionRepository.CheckDimensionIdIsUsedInStructure(dimensionIds);
            var data = _mapper.Map<List<DimensionCheckDto>>(existDatas);
            return data;
        }

        public async Task<ApiResponse<bool>> Delete(AnalysisDimensionDeleteModel model, string name)
        {
            var existDatas = await _analysisDimensionRepository.CheckDimensionIdIsUsedInStructure(model.DimensionIds);
            if (existDatas.Count > 0)
                return ApiResponse<bool>.Fail(existDatas, 400);

            int userId = await _userRepository.ConvertIdentity(name);
            var code = false;
            int counter = 0;
            for (int i = 0; i < model.DimensionIds.Count; i++)
            {
                code = await _analysisDimensionRepository.Delete(model.DimensionIds[i], userId);
                if (code)
                    counter++;
            }

            if (counter == model.DimensionIds.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(code, 200);
            }
            return ApiResponse<bool>.Fail("Analysis code can not be deleted", 400);
        }

        public async Task<ApiResponse<bool>> Save(List<AnalysisDimensionDto> analysisDimension, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var dimension = false;
            int counter = 0;
            for (int i = 0; i < analysisDimension.Count; i++)
            {
                if (analysisDimension[i].AnalysisDimensionId < 0)
                    analysisDimension[i].AnalysisDimensionId = 0;
                dimension = await _analysisDimensionRepository.Save(analysisDimension[i], userId);
                if (dimension)
                    counter++;
            }

            if (counter == analysisDimension.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(dimension, 200);
            }
            return ApiResponse<bool>.Fail("Analysis dimension can not be saved", 400);
        }
    }
}
