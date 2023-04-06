using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Buyer;
using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BuyerService(IBuyerRepository buyerRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _buyerRepository = buyerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task AddAsync(BuyerDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> DeleteBuyerByGroupIdAsync(int groupBuyerId)
        {
            var result = await _buyerRepository.DeleteBuyerByGroupIdAsync(groupBuyerId);
            await _unitOfWork.SaveChangesAsync();
            if (result) return ApiResponse<bool>.Success(true, 200);
            else return ApiResponse<bool>.Fail("Data can not be deleted", 400);
        }

        public async Task<ApiResponse<List<BuyerDto>>> GetAllAsync()
        {
            var status = await _buyerRepository.GetAllAsync();
            var dto = _mapper.Map<List<BuyerDto>>(status);
            return ApiResponse<List<BuyerDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<BuyerDto>>> GetBuyerByUserTokenAsync(string authToken, string businessUnitCode)
        {
            var buyer = await _buyerRepository.GetBuyerByUserTokenAsync(await _userRepository.GetUserIdByTokenAsync(authToken), businessUnitCode);
            var buyerDto = _mapper.Map<List<BuyerDto>>(buyer);

            if (buyerDto != null)
                return ApiResponse<List<BuyerDto>>.Success(buyerDto, 200);

            return ApiResponse<List<BuyerDto>>.Fail("Buyer is empty", 400);
        }

        public async Task<ApiResponse<List<GroupBuyerDto>>> GetBuyersByGroupIdAsync(int groupId)
        {
            var buyers = await _buyerRepository.GetBuyersByGroupIdAsync(groupId);
            var dto = _mapper.Map<List<GroupBuyerDto>>(buyers);
            if (dto != null)
                return ApiResponse<List<GroupBuyerDto>>.Success(dto, 200);
            else
                return ApiResponse<List<GroupBuyerDto>>.Fail("Buyer list is empty", 400);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> SaveBuyerByGroupAsync(GroupBuyerSaveModel model)
        {
            var res = await _buyerRepository.SaveBuyerByGroupAsync(model);
            await _unitOfWork.SaveChangesAsync();
            if (res)
                return ApiResponse<bool>.Success(res, 200);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public Task<ApiResponse<bool>> UpdateAsync(BuyerDto model)
        {
            throw new NotImplementedException();
        }
    }
}
