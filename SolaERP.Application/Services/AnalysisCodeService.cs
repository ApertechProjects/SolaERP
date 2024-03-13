using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System.Collections.Generic;
using System.Net;

namespace SolaERP.Persistence.Services
{
    public class AnalysisCodeService : IAnalysisCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAnalysisStructureRepository _analysisCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IAnalysisDimensionRepository _dimensionRepository;

        public AnalysisCodeService(IMapper mapper,
                                   IUnitOfWork unitOfWork,
                                   IAnalysisStructureRepository analysisCodeRepository,
                                   IAnalysisDimensionRepository dimensionRepository,
                                   IUserRepository userRepository,
                                   IBusinessUnitRepository businessUnitRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _analysisCodeRepository = analysisCodeRepository;
            _userRepository = userRepository;
            _dimensionRepository = dimensionRepository;
            _businessUnitRepository = businessUnitRepository;
        }

        public async Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(AnalysisCodeDeleteModel model, string userName)
        {
            int userId = await _userRepository.ConvertIdentity(userName);
            var code = false;
            int counter = 0;
            for (int i = 0; i < model.CodeIds.Count; i++)
            {
                code = await _analysisCodeRepository.DeleteAnalysisCodeAsync(model.CodeIds[i], userId);
                if (code)
                    counter++;
            }

            if (counter == model.CodeIds.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(code, 200);
            }
            return ApiResponse<bool>.Fail("Analysis code can not be deleted", 400);
        }

        public async Task<ApiResponse<List<AnalysisListDto>>> GetAnalysisCodeListAsync(int dimensionId, string userName)
        {
            var userId = await _userRepository.ConvertIdentity(userName);
            var analysisCodes = await _analysisCodeRepository.GetAnalysisCodesAsync(dimensionId, userId);
            var analysisListDtos = analysisCodes.Select(x => new AnalysisListDto
            {
                AnalysisCodesId = x.AnalysisCodesId,
                AnalysisCode = x.AnalysisCode,
                AnalysisName = x.AnalysisName
            }).ToList();

            if (analysisListDtos.Count == 0)
                return ApiResponse<List<AnalysisListDto>>.Fail("Data not found", 404);
            return ApiResponse<List<AnalysisListDto>>.Success(analysisListDtos.OrderBy(x => x.AnalysisCode).ToList());
        }

        public async Task<ApiResponse<AnalysisCodeModel>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest)
        {
            var analysisCodes = await _analysisCodeRepository.GetAnalysisCodesAsync(getRequest.BusinessUnitId, getRequest.ProcedureName);
            var analysisCodeByCat = analysisCodes.Where(x => x.CatId == getRequest.CatId).FirstOrDefault();

            if (analysisCodeByCat == null)
                return ApiResponse<AnalysisCodeModel>.Fail("Analysis codes is empty", 404);

            AnalysisCodeModel analysisCodeModel = new AnalysisCodeModel()
            {
                CatId = analysisCodeByCat.CatId,
                AnalysisCodes = new List<Application.Dtos.AnalysisCode.AnalysisCode>
                {
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId1,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode1
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId2,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode2
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId3,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode3
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId4,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode4
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId5,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode5
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId6,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode6
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId7,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode7
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId8,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode8
                    },
                    new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId9,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode9
                    },
                        new Application.Dtos.AnalysisCode.AnalysisCode
                    {
                        AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId10,
                        AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode10
                    },
                }
            };

            return ApiResponse<AnalysisCodeModel>.Success(analysisCodeModel, 200);

        }

        public async Task<ApiResponse<List<AnalysisDto>>> GetAnalysisCodesAsync(int dimensionId, string userName)
        {
            var userId = await _userRepository.ConvertIdentity(userName);
            var analysisCodes = await _analysisCodeRepository.GetAnalysisCodesAsync(dimensionId, userId);

            var analysisDimension = await _dimensionRepository.ByAnalysisDimensionId(dimensionId, userId);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(Convert.ToInt32(analysisDimension?.BusinessUnitId));

            var map = analysisCodes?.Select(ac => new AnalysisDto
            {
                AnalysisCodesId = ac.AnalysisCodesId,
                BusinessUnitId = businessUnit.BusinessUnitId,
                BusinessUnitName = businessUnit?.BusinessUnitName,
                AnalysisDimensionCode = analysisDimension?.AnalysisDimensionCode,
                AnalysisDimensionId = analysisDimension.AnalysisDimensionId,
                AnalysisCode = ac?.AnalysisCode,
                AnalysisName = ac?.AnalysisName,
                Status = ac.Status,
                Description = ac?.Description,
                AdditionalDescription = ac?.AdditionalDescription,
                AdditionalDescription2 = ac?.AdditionalDescription2,
                LinkedAnalysisDimensionid = ac.LinkedAnalysisDimensionid,
                IsLinked = ac.IsLinked,
                Date1 = ac.Date1,
                Date2 = ac.Date2,
                IsModified = ac.IsModified,
            }).ToList();

            return ApiResponse<List<AnalysisDto>>.Success(map, 200);
        }

        public async Task<ApiResponse<List<AnalysisCodesModel>>> GetAnalysisCodesListAsync(AnalysisCodeGetModel getRequest, string userName)
        {
            var analysisCodes = await _analysisCodeRepository.GetAnalysisCodesAsync(getRequest.BusinessUnitId, getRequest.ProcedureName);
            var analysisCodeByCat = analysisCodes.Where(x => x.CatId == getRequest.CatId).FirstOrDefault();

            if (analysisCodeByCat == null)
                return ApiResponse<List<AnalysisCodesModel>>.Fail("Analysis codes is empty", 404);

            List<AnalysisCodesModel> analysisCodesModels = new List<AnalysisCodesModel>()
            {
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId1,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode1,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId1,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId2,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode2,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId2,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId3,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode3,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId3,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId4,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode4,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId4,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId5,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode5,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId5,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId6,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode6,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId6,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId7,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode7,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId7,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId8,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode8,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId8,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId9,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode9,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId9,userName).Result.Data
                },
                new AnalysisCodesModel
                {
                    AnalysisDimensionId = analysisCodeByCat.AnalysisDimensionId10,
                    AnalysisDimensionCode = analysisCodeByCat.AnalysisDimensionCode10,
                    AnalysisCodes = GetAnalysisCodeListAsync(analysisCodeByCat.AnalysisDimensionId10,userName).Result.Data
                },
            };

            return ApiResponse<List<AnalysisCodesModel>>.Success(analysisCodesModels, 200);

        }

        public async Task<ApiResponse<List<AnalysisWithBuDto>>> GetByBUIdAsync(int businessUnitId, string userName)
        {
            var userId = await _userRepository.ConvertIdentity(userName);
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
            int userId = await _userRepository.ConvertIdentity(name);
            var code = false;
            int counter = 0;
            for (int i = 0; i < analysisCodeSave.Count; i++)
            {
                if (analysisCodeSave[i].AnalysisCodesId < 0)
                    analysisCodeSave[i].AnalysisCodesId = 0;
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
