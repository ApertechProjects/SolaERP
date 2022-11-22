using AutoMapper;
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
        private IMapper _mapper;

        public BusinessUnitService(IBusinessUnitRepository businessUnitRepository,IMapper mapper)
        {
            _businessUnitRepository = businessUnitRepository;
            _mapper = mapper;
        }

        public Task<ApiResponse<Token>> AddAsync(BusinessUnitsDto model)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<List<BusinessUnitsDto>> GetAll()
        {
            var businessUnits = _businessUnitRepository.GetAllAsync();
            var dto = _mapper.Map<List<BusinessUnitsDto>>(businessUnits);

            return ApiResponse<List<BusinessUnitsDto>>.Success(dto, 200);
        }
    }
}
