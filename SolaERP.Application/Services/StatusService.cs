using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private IMapper _mapper;
        public StatusService(IStatusRepository statusRepository,IMapper mapper)
        {
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        public Task AddAsync(StatusDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<StatusDto>>> GetAllAsync()
        {
            var status = await _statusRepository.GetAllAsync();
            var dto = _mapper.Map<List<StatusDto>>(status);
            return ApiResponse<List<StatusDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(StatusDto model)
        {
            throw new NotImplementedException();
        }
    }
}
