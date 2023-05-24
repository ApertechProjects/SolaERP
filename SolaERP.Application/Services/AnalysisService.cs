using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.AnalysisStructure;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewAnalysisStructureRepository _repository;

        public AnalysisService(INewAnalysisStructureRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(AnalysisStructureInputModel model)
        {
            bool result = await _repository.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<AnalysisStructureWithBu> GetByBUAsync(int buId)
        {
            return await _repository.GetByBUAsync(buId);
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

        public async Task<bool> UpdateAsync(AnalysisStructureUpdateModel model)
        {
            var result = await _repository.UpdateAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
