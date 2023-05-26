using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Dtos.Language;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Translate;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Entities.Translate;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ILanguageRepository
    {
        Task<List<Language>> GetLanguagesLoadAsync();
        Task<bool> SaveLanguageAsync(Language language);
        Task<bool> DeleteLanguageAsync(int id);
        Task<List<Translate>> GetTranslatesLoadByLanguageCodeAsync(string code);
        Task<bool> SaveTranslateAsync(TranslateDto translate);
        Task<bool> DeleteTranslateAsync(int id);
        Task<List<Translate>> GetTranslatesLoadAsync();
        Task<bool> CheckDuplicateTranslate(string languageCode, string key);
    }
}
