using SolaERP.Infrastructure.Entities.Attachment;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IAttachmentRepository
    {
        public Task<List<Attachment>> GetAttachmentListAsync(AttachmentListGetModel model);
        public Task<List<Attachment>> GetAttachmenListWithFileDataAsync(int attachmentId);
        public Task<bool> SaveAttachmentAsync(AttachmentSaveModel attachment);
    }
}
