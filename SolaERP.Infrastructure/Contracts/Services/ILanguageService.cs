using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Dtos.Shared;
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
        Task<ApiResponse<LanguageDto>> GetLanguagesLoad();
        Task<ApiResponse<bool>> SaveLanguageAsync(string finderToken, LayoutDto layout);
        Task<ApiResponse<bool>> DeleteLanguageAsync(string finderToken, LayoutDeleteModel layout);
    }
}
