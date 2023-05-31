using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IAttachmentRepository
    {
        public Task<List<Attachment>> GetAttachmentListAsync(int sourceId, string reference, string sourceType);
        public Task<List<Attachment>> GetAttachmenListWithFileDataAsync(int attachmentId);
        public Task<bool> SaveAttachmentAsync(AttachmentSaveModel attachment);
        Task<bool> DeleteAttachmentAsync(int attachmentId);
    }
}
