using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using System.Reflection;

namespace SolaERP.Persistence.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration _configuration;
        public FileUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> UploadFile(List<IFormFile> Files, Modules Module, string BearerToken)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

                foreach (var item in Files)
                {
                    string fileName = item.FileName;

                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await item.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    var fileContent = new ByteArrayContent(fileBytes);
                    content.Add(new StringContent(Module.ToString()), "module");
                    content.Add(fileContent, "files", fileName);
                }

                var response = await client.PostAsync(_configuration["FileOptions:BaseUrl"] + "api/v1/home/upload", content);
                var tttt = response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }


    }


}

