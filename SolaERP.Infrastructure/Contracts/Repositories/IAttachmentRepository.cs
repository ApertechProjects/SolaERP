using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAttachmentRepository
    {
        public Task<List<Attachment>> GetAttachmentsAsync(int sourceId, string reference, string sourceType, int attachemntTypeId = 0);
        public Task<List<Attachment>> GetAttachmentsWithFileDataAsync(int attachmentId);
        public Task<bool> SaveAttachmentAsync(AttachmentSaveModel attachment);
        Task<bool> DeleteAttachmentAsync(int attachmentId);
    }
}
