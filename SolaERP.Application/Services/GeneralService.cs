using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class GeneralService : IGeneralService
    {
        private readonly IGeneralRepository _generalRepository;
        private IMapper _mappper;
        public GeneralService(IGeneralRepository generalRepository, IMapper mapper)
        {
            _generalRepository = generalRepository;
            _mappper = mapper;
        }

        public async Task<ApiResponse<List<RejectReasonDto>>> RejectReasons()
        {
            var data = await _generalRepository.RejectReasons();
            var dto = _mappper.Map<List<RejectReasonDto>>(data);

            if (dto.Count > 0)
                return ApiResponse<List<RejectReasonDto>>.Success(dto, 200);
            return ApiResponse<List<RejectReasonDto>>.Fail("Data not found", 404);
        }
    }
}
