using Microsoft.AspNetCore.Http;
using SolaERP.Application.Contracts.Repositories;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileProducer
    {
        Task ProduceAsync(IFormFile file, Filetype type, string email);

    }
}
