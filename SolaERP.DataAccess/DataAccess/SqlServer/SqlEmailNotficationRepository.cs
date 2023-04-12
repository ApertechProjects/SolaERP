using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Email;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlEmailNotficationRepository : IEmailNotficationRepository
    {
        private IUnitOfWork _unitOfWork;

        public SqlEmailNotficationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(EmailNotfication model)
        {
            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "SET NOCOUNT OFF exec SP_EmailNotification_IUD @EmailNotId,@Notification,@Description";

            command.Parameters.AddWithValue(command, "@EmailNotId", 0);
            command.Parameters.AddWithValue(command, "@Notification", model.Notification);
            command.Parameters.AddWithValue(command, "@Description", model.Description);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<EmailNotfication>> GetAllEmailNotficationsAsync()
        {
            List<EmailNotfication> emailNotfications = new();

            using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "exec SP_EmailNotification_Load";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) emailNotfications.Add(reader.GetByEntityStructure<EmailNotfication>());

            return emailNotfications;
        }

        public async Task<bool> UpdateAsync(EmailNotfication model)
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
    }
}
