using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Attachment;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

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
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_Attachments_IUD @AttachmentId,@FileName,@FileData,@SourceId,@SourceType,@Reference,@ExtensionType,@AttachmentTypeId,@AttachmentSubTypeId,@UploadDateTime,@Size";
                command.Parameters.AddWithValue(command, "@AttachmentId", attachment.AttachmentId);
                command.Parameters.AddWithValue(command, "@FileName", attachment.FileName);
                command.Parameters.AddWithValue(command, "@FileData", attachment.FileData);
                command.Parameters.AddWithValue(command, "@SourceId", attachment.SourceId);
                command.Parameters.AddWithValue(command, "@SourceType", attachment.SourceType);
                command.Parameters.AddWithValue(command, "@Reference", attachment.Reference);
                command.Parameters.AddWithValue(command, "@ExtensionType", attachment.ExtensionType);
                command.Parameters.AddWithValue(command, "@AttachmentTypeId", attachment.AttachmentTypeId);
                command.Parameters.AddWithValue(command, "@AttachmentSubTypeId", attachment.AttachmentSubTypeId);
                command.Parameters.AddWithValue(command, "@UploadDateTime", DateTime.Now);
                command.Parameters.AddWithValue(command, "@Size", attachment.Size);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<Attachment>> GetAttachmenListWithFileDataAsync(int attachmentId)
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

        public async Task<List<Attachment>> GetAttachmentListAsync(AttachmentListGetModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_AttachmentList_Load @SourceId,@Reference,@SourceType";
                command.Parameters.AddWithValue(command, "@SourceId", model.SourceId);
                command.Parameters.AddWithValue(command, "@Reference", model.Reference);
                command.Parameters.AddWithValue(command, "@SourceType", model.Sorucetype);

                using var reader = await command.ExecuteReaderAsync();

                List<Attachment> attachments = new List<Attachment>();

                while (await reader.ReadAsync()) { attachments.Add(reader.GetByEntityStructure<Attachment>("FileData")); }
                return attachments;
            }
        }
    }
}
