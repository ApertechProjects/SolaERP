using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using System.Text;

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
            try
            {
                using (var command = _unitOfWork.CreateCommand() as DbCommand)
                {
                    command.CommandText = "SET NOCOUNT OFF EXEC SP_Attachments_IUD @AttachmentId,@FileName,@FileData,@SourceId,@SourceType,@Reference,@ExtensionType,@AttachmentTypeId,@AttachmentSubTypeId,@UploadDateTime,@Size";
                    command.Parameters.AddWithValue(command, "@AttachmentId", attachment.AttachmentId);
                    command.Parameters.AddWithValue(command, "@FileName", attachment.Name);
                    command.Parameters.AddWithValue(command, "@FileData", attachment.Filebase64);
                    command.Parameters.AddWithValue(command, "@SourceId", attachment.SourceId);
                    command.Parameters.AddWithValue(command, "@SourceType", attachment.SourceType);
                    command.Parameters.AddWithValue(command, "@Reference", null);
                    command.Parameters.AddWithValue(command, "@ExtensionType", attachment.ExtensionType);
                    command.Parameters.AddWithValue(command, "@AttachmentTypeId", attachment.AttachmentTypeId);
                    command.Parameters.AddWithValue(command, "@AttachmentSubTypeId", attachment.AttachmentSubTypeId);
                    command.Parameters.AddWithValue(command, "@UploadDateTime", DateTime.UtcNow.Date);
                    command.Parameters.AddWithValue(command, "@Size", attachment.Size);

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(attachment.Name);
                await Console.Out.WriteLineAsync(attachment.SourceId.ToString());
                await Console.Out.WriteLineAsync(attachment.SourceType);
                return false;
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
    }
}
