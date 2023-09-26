using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAttachmentRepository
    {
        public Task<List<Attachment>> GetAttachmentsAsync(int sourceId, string reference, string sourceType,
            int? attachemntTypeId = null);
        public Task<Attachment> GetAttachmentByIdAsync(int attachmentId);
        public Task<bool> SaveAttachmentAsync(AttachmentSaveModel attachment);
        Task<bool> DeleteAttachmentAsync(int attachmentId);
        Task<bool> DeleteAttachmentAsync(int sourceId, SourceType sourceType);
    }
}