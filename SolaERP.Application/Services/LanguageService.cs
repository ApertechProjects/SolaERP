using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Language;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Translate;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Entities.Translate;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public LanguageService(ILanguageRepository languageRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> DeleteLanguageAsync(int id)
        {
            bool result = await _languageRepository.DeleteLanguageAsync(id);
            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<bool>.Success(result, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<bool>> DeleteTranslateAsync(int id)
        {
            bool result = await _languageRepository.DeleteTranslateAsync(id);
            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<bool>.Success(result, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<List<LanguageDto>>> GetLanguagesLoadAsync()
        {
            var languages = await _languageRepository.GetLanguagesLoadAsync();
            var dto = _mapper.Map<List<LanguageDto>>(languages);
            return ApiResponse<List<LanguageDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<TranslateDto>>> GetTranslatesLoadAsync()
        {
            var translates = await _languageRepository.GetTranslatesLoadAsync();
            var dto = _mapper.Map<List<TranslateDto>>(translates);
            return ApiResponse<List<TranslateDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<TranslateDto>>> GetTranslatesLoadByLanguageCodeAsync(string code)
        {
            var translates = await _languageRepository.GetTranslatesLoadByLanguageCodeAsync(code);
            var dto = _mapper.Map<List<TranslateDto>>(translates);
            return ApiResponse<List<TranslateDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> SaveLanguageAsync(LanguageDto language)
        {
            var entity = _mapper.Map<Language>(language);
            bool result = await _languageRepository.SaveLanguageAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<bool>.Success(result, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<bool>> SaveTranslateAsync(TranslateDto translate)
        {
            bool CheckDuplicate = await _languageRepository.CheckDuplicateTranslate(translate.LanguageCode, translate.Key);
            if (CheckDuplicate)
                return ApiResponse<bool>.Fail("This data already exist", 400);

            bool result = await _languageRepository.SaveTranslateAsync(translate);
            await _unitOfWork.SaveChangesAsync();

            if (result)
                return ApiResponse<bool>.Success(result, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }
    }
}
