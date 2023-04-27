using SolaERP.Application.Dtos.Language;
using SolaERP.Application.Dtos.Layout;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Translate;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface ILanguageService
    {
        Task<ApiResponse<List<LanguageDto>>> GetLanguagesLoadAsync();
        Task<ApiResponse<bool>> SaveLanguageAsync(LanguageDto language);
        Task<ApiResponse<bool>> DeleteLanguageAsync(int id);
        Task<ApiResponse<List<TranslateDto>>> GetTranslatesLoadByLanguageCodeAsync(string code);
        Task<ApiResponse<bool>> SaveTranslateAsync(TranslateDto translate);
        Task<ApiResponse<bool>> DeleteTranslateAsync(int id);
        Task<ApiResponse<List<TranslateDto>>> GetTranslatesLoadAsync();
    }
}
