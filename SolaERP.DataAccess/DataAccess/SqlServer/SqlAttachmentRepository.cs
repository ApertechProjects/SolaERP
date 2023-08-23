using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using Attachment = SolaERP.Application.Entities.Attachment.Attachment;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlAttachmentRepository : IAttachmentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlAttachmentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SaveAttachmentAsync(AttachmentSaveModel attachment)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {

                command.CommandText = "SET NOCOUNT OFF EXEC SP_Attachments_IUD @AttachmentId,@FileName,@FileData,@SourceId,@SourceType,@Reference,@ExtensionType,@AttachmentTypeId,@AttachmentSubTypeId,@UploadDateTime,@Size,@FileLink";
                command.Parameters.AddWithValue(command, "@AttachmentId", attachment.AttachmentId);
                command.Parameters.AddWithValue(command, "@FileName", attachment.Name);
                command.Parameters.AddWithValue(command, "@FileData", attachment.FileLink);
                command.Parameters.AddWithValue(command, "@SourceId", attachment.SourceId);
                command.Parameters.AddWithValue(command, "@SourceType", attachment.SourceType);
                command.Parameters.AddWithValue(command, "@Reference", null);
                command.Parameters.AddWithValue(command, "@ExtensionType", attachment.ExtensionType);
                command.Parameters.AddWithValue(command, "@AttachmentTypeId", attachment.AttachmentTypeId);
                command.Parameters.AddWithValue(command, "@AttachmentSubTypeId", attachment.AttachmentSubTypeId);
                command.Parameters.AddWithValue(command, "@UploadDateTime", DateTime.UtcNow.Date);
                command.Parameters.AddWithValue(command, "@Size", attachment.Size);
                command.Parameters.AddWithValue(command, "@FileLink", attachment.FileLink);

                bool result = await command.ExecuteNonQueryAsync() > 0;

                return result;
            }

        }

        public async Task<List<Attachment>> GetAttachmentsWithFileDataAsync(int attachmentId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_Attachment_Load @AttachmentId";
                command.Parameters.AddWithValue(command, "@AttachmentId", attachmentId);

                using var reader = await command.ExecuteReaderAsync();
                List<Attachment> attachments = new List<Attachment>();

                while (await reader.ReadAsync()) { attachments.Add(reader.GetByEntityStructure<Attachment>()); }
                return attachments;
            }
        }

        public async Task<List<Attachment>> GetAttachmentsAsync(int sourceId, string reference, string sourceType, int? attachmentTypeId = null)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_AttachmentList_Load @SourceId,@Reference,@SourceType,@AttachmentTypeId";
                command.Parameters.AddWithValue(command, "@SourceId", sourceId);
                command.Parameters.AddWithValue(command, "@Reference", reference);
                command.Parameters.AddWithValue(command, "@SourceType", sourceType);
                command.Parameters.AddWithValue(command, "@AttachmentTypeId", attachmentTypeId);

                using var reader = await command.ExecuteReaderAsync();

                List<Attachment> attachments = new List<Attachment>();

                while (await reader.ReadAsync()) { attachments.Add(reader.GetByEntityStructure<Attachment>("FileData")); }
                if (attachments.Count == 0)
                    return Enumerable.Empty<Attachment>().ToList();
                return attachments;
            }
        }

        public async Task<List<string>> GetAttachmentsAsync(int sourceId, int sourceType)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from [dbo].[FN_GetAttachment] (@SourceId, @SourceType)";
                command.Parameters.AddWithValue(command, "@SourceId", sourceId);
                command.Parameters.AddWithValue(command, "@SourceType", sourceType);

                using var reader = await command.ExecuteReaderAsync();

                List<string> attachments = new List<string>();

                while (await reader.ReadAsync()) { attachments.Add(reader.Get<string>("FileData")); }
                return attachments;
            }
        }

        public async Task<bool> DeleteAttachmentAsync(int attachmentId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_Attachments_IUD @AttachmentId";
                command.Parameters.AddWithValue(command, "@AttachmentId", attachmentId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAttachmentAsync(int sourceId, SourceType sourceType)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_DeleteAttachment @SourceId,@SourceType";
                command.Parameters.AddWithValue(command, "@SourceId", sourceId);
                command.Parameters.AddWithValue(command, "@SourceType", sourceType);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
