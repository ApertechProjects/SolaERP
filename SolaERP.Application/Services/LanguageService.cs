using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.BusinessUnit;
using SolaERP.Infrastructure.Dtos.Language;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Translate;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Language;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Entities.Translate;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
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
            var entity = _mapper.Map<Translate>(translate);
            bool result = await _languageRepository.SaveTranslateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<bool>.Success(result, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }
    }
}
