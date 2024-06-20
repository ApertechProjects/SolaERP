using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UserReport;
using SolaERP.Application.Entities.UserReport;
using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class UserReportService : IUserReportService
    {
        private readonly IUserReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UserReportService(IUserReportRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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


    }
}
