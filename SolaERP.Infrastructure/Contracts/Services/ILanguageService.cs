using SolaERP.Infrastructure.Dtos.Language;
using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Translate;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ILanguageService
    {
        Task<ApiResponse<List<LanguageDto>>> GetLanguagesLoadAsync();
        Task<ApiResponse<bool>> SaveLanguageAsync(LanguageDto language);
        Task<ApiResponse<bool>> DeleteLanguageAsync(int id);
        Task<ApiResponse<List<TranslateDto>>> GetTranslatesLoadByLanguageCodeAsync(string code);
        Task<ApiResponse<bool>> SaveTranslateAsync(TranslateDto translate);
        Task<ApiResponse<bool>> DeleteTranslateAsync(int id);
    }
}
