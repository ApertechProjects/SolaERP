using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using System.Reflection;
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

        public async Task<bool> DeleteFile(Modules Module, string FileName, string Token)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                var response = await client.DeleteAsync(_configuration["FileOptions:BaseUrl"] + $"/api/v1/home/module/{Module.ToString()}/fileName/{FileName}");
                var res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<ApiResponse<List<string>>> FileOperation(List<IFormFile> Files, List<string> DeletedFiles, Modules Module, string Token)
        {
            foreach (var item in DeletedFiles)
            {
                await DeleteFile(Modules.Users, item, Token);
            }
            if (Files != null)
            {
                var Data = await UploadFile(Files, Modules.Users, Token);
                if (!string.IsNullOrEmpty(Data.Item2))
                    return ApiResponse<List<string>>.Fail(Data.Item2, 400);
                return ApiResponse<List<string>>.Success(Data.Item1.data.ToList());
            }
            return null;
        }

        public async Task<(UploadFile, string)> UploadFile(List<IFormFile> Files, Modules Module, string BearerToken)
        {
            UploadFile member = new UploadFile();
            string errorMessage = null;
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
                content.Add(new StringContent(Module.ToString()), "module");

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
                    content.Add(fileContent, "files", fileName);
                }

                var response = await client.PostAsync(_configuration["FileOptions:BaseUrl"] + "api/v1/home/upload", content);
                var res = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(res))
                {
                    try
                    {
                        member = JsonSerializer.Deserialize<UploadFile>(res);
                        errorMessage = null;
                    }
                    catch (Exception ex)
                    {
                        member = null;
                        errorMessage = res;
                    }

                }
                return (member, errorMessage);
            }
        }


    }


}

