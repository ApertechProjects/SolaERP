using Microsoft.Extensions.Configuration;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Report;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UserReport;
using SolaERP.Application.Entities.UserReport;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class UserReportService : IUserReportService
    {
        private readonly IUserReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public UserReportService(IUserReportRepository repository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<ApiResponse<bool>> Save(UserReportSaveDto data)
        {
            var delete = await _repository.Delete(data.ReportFileId);
            foreach (var item in data.Users)
            {
                UserReportFileAccess userReportSave = new UserReportFileAccess
                {
                    Id = data.Id,
                    ReportFileId = data.ReportFileId,
                    ReportFileName = data.ReportFileName,
                    UserId = item,
                };
                var result = await _repository.Save(userReportSave);
            }
            _unitOfWork.SaveChanges();

            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> SaveAs(string dashboardId)
        {
            string fileName = await GetFileName();
            bool result = await CopyFile(dashboardId, fileName);
            if (result)
                return ApiResponse<bool>.Success(true);
            return ApiResponse<bool>.Fail("Error", 400);
        }

        private async Task<bool> CopyFile(string dashboardId, string fileName)
        {
            try
            {
                string sourceFilePath = _configuration["FileOptions:ReportPath"] + "/" + dashboardId + ".xml";
                string destinationFilePath = _configuration["FileOptions:ReportPath"] + "/" + fileName;


                File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
            }

            catch (IOException ioEx)
            {
                Console.WriteLine("An error occurred: " + ioEx.Message);
                Debug.WriteLine("An error occurred: " + ioEx.Message);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine("Access error: " + uaEx.Message);
                Debug.WriteLine("Access error: " + uaEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
                Debug.WriteLine("An unexpected error occurred: " + ex.Message);
            }
            return false;
        }

        private async Task<string> GetFileName()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = _configuration["FileOptions:DashboardUrl"] + "api/dashboard/dashboards";

                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();
                    string id = null;
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var reports = JsonSerializer.Deserialize<List<ReportDto>>(responseBody, options);
                    var report = reports.OrderByDescending(x => x.Id).ToList().FirstOrDefault();
                    if (report == null)
                        throw new Exception("Save as is not permitted");

                    id = report.Id;
                    var lastIndexString = id.Substring(id.Length - 1, 1);
                    var lastIndex = Convert.ToInt16(lastIndexString) + 1;
                    var fileName = $"dashboard{lastIndex}.xml";
                    return fileName;
                }
                catch (HttpRequestException e)
                {

                }
                catch (Exception ex)
                {

                }
                return null;
            }
        }


    }
}
