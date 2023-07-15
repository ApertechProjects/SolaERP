using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Account;
using SolaERP.Application.Dtos.GridLayout;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.AccountCode;
using SolaERP.Application.Entities.GridLayout;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class GridLayoutService : IGridLayoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGridLayoutRepository _gridLayoutRepository;
        private IMapper _mapper;

        public GridLayoutService(IUnitOfWork unitOfWork, IGridLayoutRepository gridLayoutRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gridLayoutRepository = gridLayoutRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(GridLayoutDto model)
        {
            var entity = _mapper.Map<GridLayout>(model);
            await _gridLayoutRepository.AddAsync(entity);

            _unitOfWork.SaveChanges();
        }

        public async Task<ApiResponse<List<GridLayoutDto>>> GetAllAsync()
        {
            var layouts = await _gridLayoutRepository.GetAllAsync();
            var dto = _mapper.Map<List<GridLayoutDto>>(layouts);
            return ApiResponse<List<GridLayoutDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<GridLayoutDto>> GetAsync(int userId, string layoutName)
        {
            var layout = await _gridLayoutRepository.GetAsync(userId, layoutName);
            var dto = _mapper.Map<GridLayoutDto>(layout);
            return ApiResponse<GridLayoutDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> RemoveAsync(int id)
        {
            await _gridLayoutRepository.RemoveAsync(id);
            _unitOfWork.SaveChanges();
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(GridLayoutDto model)
        {
            var entity = _mapper.Map<GridLayout>(model);
            await _gridLayoutRepository.UpdateAsync(entity);
            _unitOfWork.SaveChanges();

            return ApiResponse<bool>.Success(true);
        }
    }
}
