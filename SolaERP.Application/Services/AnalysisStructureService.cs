using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class AnalysisStructureService : IAnalysisStructureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewAnalysisStructureRepository _repository;
        private readonly IUserService _userService;

        public AnalysisStructureService(INewAnalysisStructureRepository repository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<bool> AddAsync(AnalysisStructureSaveModel model)
        {
            bool result = await _repository.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<AnalysisStructureWithBu> GetByBUAsync(int buId, int procedureId, string userName)
        {
            int userId = await _userService.GetIdentityNameAsIntAsync(userName);
            return await _repository.GetByBUAsync(buId, procedureId, userId);
        }

        public async Task<AnalysisStructure> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(int id, int userId)
        {
            var result = await _repository.RemoveAsync(id, userId);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<bool> UpdateAsync(AnalysisStructureDeleteModel model)
        {
            var result = await _repository.UpdateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
