using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using System.Text.Json;

namespace SolaERP.Persistence.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration _configuration;
        public FileUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> DeleteFile(Modules module, string fileName, string token)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(_configuration["FileOptions:BaseUrl"] + $"api/v1/home/module/{module.ToString()}/fileName/{fileName}");
                var res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<ApiResponse<List<string>>> AddFile(List<IFormFile> files, List<string> deletedFiles, Modules module, string token)
        {
            if (files != null)
            {
                var NotNullFileCount = files.Where(x => x?.FileName != null).Count();
                if (NotNullFileCount != 0)
                {
                    var Data = await UploadFile(files, module, token);

                    foreach (var item in deletedFiles) //if upload operation is correct, then delete old files
                    {
                        await DeleteFile(module, item, token);
                    }
                    return ApiResponse<List<string>>.Success(Data.Item1.data?.ToList());
                }
            }
            return ApiResponse<List<string>>.Fail(null, 400);
        }

        public string GetFileLink(string FileName, Modules module, string Token)
        {
            if (!string.IsNullOrEmpty(FileName))
                return _configuration["FileOptions:BaseUrl"] + $"api/v1/home/module/{module}/fileName/{FileName}";
            return null;
        }

        public async Task<(UploadFile, string)> UploadFile(List<IFormFile> files, Modules module, string token)
        {
            UploadFile member = new UploadFile();
            string errorMessage = null;
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                content.Add(new StringContent(module.ToString()), "module");

                foreach (var item in files)
                {
                    if (item is null)
                        continue;

                    string fileName = item.FileName;

                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await item.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    var fileContent = new ByteArrayContent(fileBytes);
                    content.Add(fileContent, "files", fileName);
                }

                var response = await client.PostAsync(_configuration["FileOptions:BaseUrl"] + "api/v1/home/upload", content);
                var res = await response.Content.ReadAsStringAsync();

                try
                {
                    member = JsonSerializer.Deserialize<UploadFile>(res);
                    errorMessage = null;
                }
                catch (Exception ex)
                {
                    member = null;
                    throw new Exception(res);
                }

                return (member, errorMessage);
            }
        }


    }


}

