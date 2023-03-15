using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Language;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Translate;
using SolaERP.Infrastructure.Entities.Language;
using SolaERP.Infrastructure.Entities.Translate;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ILanguageRepository
    {
        Task<List<Language>> GetLanguagesLoadAsync();
        Task<bool> SaveLanguageAsync(Language language);
        Task<bool> DeleteLanguageAsync(int id);
        Task<List<Translate>> GetTranslatesLoadByLanguageCodeAsync(string code);
        Task<bool> SaveTranslateAsync(Translate translate);
        Task<bool> DeleteTranslateAsync(int id);
        Task<List<Translate>> GetTranslatesLoadAsync();
    }
}
