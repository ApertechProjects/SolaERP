using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Enums;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using Language = SolaERP.Application.Enums.Language;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlEmailNotificationRepository : IEmailNotificationRepository
    {
        private IUnitOfWork _unitOfWork;

        public SqlEmailNotificationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(EmailNotification model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "SET NOCOUNT OFF exec SP_EmailNotification_IUD @EmailNotId,@Notification,@Description";

            command.Parameters.AddWithValue(command, "@EmailNotId", 0);
            command.Parameters.AddWithValue(command, "@Notification", model.Notification);
            command.Parameters.AddWithValue(command, "@Description", model.Description);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<EmailNotification>> GetAllEmailNotificationsAsync()
        {
            List<EmailNotification> emailNotifications = new();

            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "exec SP_EmailNotification_Load";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) emailNotifications.Add(reader.GetByEntityStructure<EmailNotification>());

            return emailNotifications;
        }

        public async Task<bool> UpdateAsync(EmailNotification model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "SET NOCOUNT OFF exec SP_EmailNotification_IUD @EmailNotId,@Notification,@Description";

            command.Parameters.AddWithValue(command, "@EmailNotId", model.EmailNotificationId);
            command.Parameters.AddWithValue(command, "@Notification", model.Notification);
            command.Parameters.AddWithValue(command, "@Description", model.Description);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "SET NOCOUNT OFF exec SP_EmailNotification_IUD @EmailNotId,@Notification,@Description";

            command.Parameters.AddWithValue(command, "@EmailNotId", id);
            command.Parameters.AddWithValue(command, "@Notification", null);
            command.Parameters.AddWithValue(command, "@Description", null);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<string> GetCompanyName(string email)
        {
            string result = string.Empty;
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "select * from VW_CompanyName";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                result = reader.Get<string>("CompanyName");
            return result;
        }

        public async Task<EmailTemplateData> GetEmailTemplateData(Language language, EmailTemplateKey templateKey)
        {
            EmailTemplateData emailTemplate = new();

            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "select * from FN_GetMailTemplateData(@TemplateKey,@Language)";
            command.Parameters.AddWithValue(command, "@TemplateKey", templateKey.ToString());
            command.Parameters.AddWithValue(command, "@Language", language.ToString());

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) emailTemplate = reader.GetByEntityStructure<EmailTemplateData>();

            return emailTemplate;
        }

        public async Task<List<EmailTemplateData>> GetEmailTemplateData(EmailTemplateKey templateKey)
        {
            List<EmailTemplateData> templates = new List<EmailTemplateData>();
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "select * from FN_GetEmailTemplateByEmailType(@TemplateKey)";
            command.Parameters.AddWithValue(command, "@TemplateKey", templateKey.ToString());

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) templates.Add(reader.GetByEntityStructure<EmailTemplateData>());

            return templates;
        }
    }
}
