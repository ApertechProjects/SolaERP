using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
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
        private IMapper _mapper;
        public AnalysisDimensionService(IAnalysisDimensionRepository analysisDimensionRepository, IUserRepository userRepository, IMapper mapper)
        {
            _analysisDimensionRepository = analysisDimensionRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AnalysisDimensionDto>>> ByAnalysisDimensionId(int analysisDimensionId, string name)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var currCodes = await _analysisDimensionRepository.ByAnalysisDimensionId(analysisDimensionId, userId);
            var dto = _mapper.Map<List<AnalysisDimensionDto>>(currCodes);
            return ApiResponse<List<AnalysisDimensionDto>>.Success(dto, 200);
        }
    }
}
