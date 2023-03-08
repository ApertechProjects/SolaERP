using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.UOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class UOMService : IUOMService
    {
        private readonly IUOMRepository _uomRepository;
        private IMapper _mapper;
        public UOMService(IUOMRepository uomRepository, IMapper mapper)
        {
            _uomRepository = uomRepository;
            _mapper = mapper;
        }
        public Task AddAsync(UOMDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<UOMDto>>> GetAllAsync()
        {
            var uoms = await _uomRepository.GetAllAsync();
            var dto = _mapper.Map<List<UOMDto>>(uoms);
            return ApiResponse<List<UOMDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(UOMDto model)
        {
            throw new NotImplementedException();
        }
    }
}
