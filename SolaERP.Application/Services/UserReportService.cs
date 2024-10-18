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
using Confluent.Kafka;
using System.Xml.Linq;

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

        public async Task<ApiResponse<bool>> Delete(string dashboardId)
        {
            var reports = await GetDashboards();
            var reportIds = reports.Data.Select(x => x.Id).ToList();
            if (!reportIds.Contains(dashboardId))
            {
                return ApiResponse<bool>.Fail("This dashboard is not exist in system", 422);
            }
            string sourceFilePath = _configuration["FileOptions:ReportPath"] + "/" + dashboardId + ".xml";
            File.Delete(sourceFilePath);
            return ApiResponse<bool>.Success(true);
        }


        public async Task<ApiResponse<bool>> SaveAs(string dashboardId, string dashboardName, string userName)
        {
            var delete = await _repository.Delete(dashboardId);

            UserReportFileAccess userReportSave = new UserReportFileAccess
            {
                Id = null,
                ReportFileId = dashboardId,
                ReportFileName = dashboardName,
                UserId = Convert.ToInt32(userName),
            };
            var result = await _repository.Save(userReportSave);
            _unitOfWork.SaveChanges();

            return ApiResponse<bool>.Success(true);
        }

        private static void ReplaceWordInFile(string inputFile, string outputFile, string fileName)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(inputFile);
                string titleText = string.Empty;
                var titleElement = xmlDoc.Element("Dashboard")?.Element("Title");
                if (titleElement != null)
                {
                    titleText = titleElement.Attribute("Text")?.Value;
                    Console.WriteLine($"Title Text: {titleText}");
                }
                else
                {
                    Console.WriteLine("Title element not found.");
                }

                // Read the contents of the file
                string fileContents = File.ReadAllText(inputFile);

                // Replace the old word with the new word
                string updatedContents = fileContents.Replace(titleText, fileName);

                // Write the updated content to a new file
                File.WriteAllText(outputFile, updatedContents);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task<bool> CopyFile(string dashboardId, string dashboardName, string fileName)
        {
            try
            {
                string sourceFilePath = _configuration["FileOptions:ReportPath"] + "/" + dashboardId + ".xml";
                string destinationFilePath = _configuration["FileOptions:ReportPath"] + "/" + fileName;
                File.Copy(sourceFilePath, destinationFilePath, overwrite: true);

                ReplaceWordInFile(sourceFilePath, destinationFilePath, dashboardName);

                return true;
            }

            catch (IOException ioEx)
            {
                Console.WriteLine("An error occurred: " + ioEx.Message);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine("Access error: " + uaEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
            return false;
        }

        public async Task<ApiResponse<List<ReportDto>>> GetDashboards()
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
                    return ApiResponse<List<ReportDto>>.Success(reports);
                }
                catch (Exception ex)
                {
                    return ApiResponse<List<ReportDto>>.Fail("Error occured", 500);
                }
                return new();
            }

        }

        private async Task<string> GetFileName(List<ReportDto> reports)
        {
            var dashboardIds = reports.Select(x => Convert.ToInt16(x.Id.Substring(9, x.Id.Length - 9))).ToList();

            if (dashboardIds == null)
                throw new Exception("Save as is not permitted");

            int maxId = dashboardIds.Max() + 1;
            var fileName = $"dashboard{maxId}.xml";
            return fileName;
        }

    }
}
