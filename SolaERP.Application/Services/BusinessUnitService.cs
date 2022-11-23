using AutoMapper;
using Microsoft.AspNetCore.Http;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Entities.BusinessUnits;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;
        IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public BusinessUnitService(IBusinessUnitRepository businessUnitRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _businessUnitRepository = businessUnitRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ApiResponse<Token>> AddAsync(BusinessUnitsAllDto model)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<List<BusinessUnitsAllDto>> GetAll()
        {
            var businessUnits = _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsAllDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsAllDto>>.Success(dto, 200);
        }

        public ApiResponse<List<BusinessUnitsDto>> GetBusinessUnitListByUserId()
        {
            var businessUnits = _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsDto>>.Success(dto,200);
        }
    }
}
