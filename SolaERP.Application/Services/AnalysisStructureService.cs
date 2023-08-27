using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnalysisStructure;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.AnalysisDimension;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System.Xml.Linq;

namespace SolaERP.Persistence.Services
{
    public class AnalysisStructureService : IAnalysisStructureService
    {
        private readonly INewAnalysisStructureRepository _repository;
        private readonly IAnalysisDimensionService _dimensionService;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public AnalysisStructureService(INewAnalysisStructureRepository repository, IAnalysisDimensionService dimensionService, IUnitOfWork unitOfWork, IUserService userService, IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _dimensionService = dimensionService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AnalysisStructureWithBuDto>>> GetByBUAsync(int buId, int procedureId, string userName)
        {
            int userId = await _userService.GetIdentityNameAsIntAsync(userName);
            var data = await _repository.GetByBUAsync(buId, procedureId, userId);
            var map = _mapper.Map<List<AnalysisStructureWithBuDto>>(data);
            if (map.Count > 0)
                return ApiResponse<List<AnalysisStructureWithBuDto>>.Success(map, 200);
            return ApiResponse<List<AnalysisStructureWithBuDto>>.Fail("Analysis Structure not found", 404);

        }


        public async Task<ApiResponse<bool>> SaveAsync(List<AnalysisStructureDto> model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var structure = false;
            int counter = 0;
            for (int i = 0; i < model.Count; i++)
            {
                model[i].AnalysisStructureId = model[i].AnalysisStructureId < 0 ? 0 : model[i].AnalysisStructureId;
                model[i].AnalysisDimensionid1 = model[i].AnalysisDimensionid1 == 0 ? null : model[i].AnalysisDimensionid1;
                model[i].AnalysisDimensionid2 = model[i].AnalysisDimensionid2 == 0 ? null : model[i].AnalysisDimensionid2;
                model[i].AnalysisDimensionid3 = model[i].AnalysisDimensionid3 == 0 ? null : model[i].AnalysisDimensionid3;
                model[i].AnalysisDimensionid4 = model[i].AnalysisDimensionid4 == 0 ? null : model[i].AnalysisDimensionid4;
                model[i].AnalysisDimensionid5 = model[i].AnalysisDimensionid5 == 0 ? null : model[i].AnalysisDimensionid5;
                model[i].AnalysisDimensionid6 = model[i].AnalysisDimensionid6 == 0 ? null : model[i].AnalysisDimensionid6;
                model[i].AnalysisDimensionid7 = model[i].AnalysisDimensionid7 == 0 ? null : model[i].AnalysisDimensionid7;
                model[i].AnalysisDimensionid8 = model[i].AnalysisDimensionid8 == 0 ? null : model[i].AnalysisDimensionid8;
                model[i].AnalysisDimensionid9 = model[i].AnalysisDimensionid9 == 0 ? null : model[i].AnalysisDimensionid9;
                model[i].AnalysisDimensionid10 = model[i].AnalysisDimensionid10 == 0 ? null : model[i].AnalysisDimensionid10;

                structure = await _repository.SaveAsync(model[i], userId);
              
            }


            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(structure, 200);
            //return ApiResponse<bool>.Fail("Analysis structure can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(AnalysisStructureDeleteModel model, string userName)
        {
            int userId = await _userRepository.ConvertIdentity(userName);
            var code = false;
            int counter = 0;
            for (int i = 0; i < model.StructureIds.Count; i++)
            {
                code = await _repository.DeleteAsync(model.StructureIds[i], userId);
                if (code)
                    counter++;
            }

            if (counter == model.StructureIds.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(code, 200);
            }
            return ApiResponse<bool>.Fail("Analysis structure can not be deleted", 400);
        }

        public async Task<bool> CheckDimensionIdIsUsed(int dimensionId)
        {
            var data = await _repository.CheckDimensionIdIsUsed(dimensionId);
            return data;
        }
    }
}
