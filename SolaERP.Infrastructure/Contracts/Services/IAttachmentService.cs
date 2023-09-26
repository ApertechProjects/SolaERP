using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAttachmentService
    {
        Task<List<AttachmentDto>> GetAttachmentsAsync(int sourceId, SourceType sourceType, Modules module,
            string reference = null, bool isDownloadLink = true);

        Task<AttachmentDto> GetAttachmentById(int attachmentId, bool getLink = false,
            Modules module = default, bool isDownloadLink = true);
        
        Task<bool> SaveAttachmentAsync(AttachmentSaveModel model);
        Task<bool> DeleteAttachmentAsync(int attachmentId);
        Task<bool> DeleteAttachmentAsync(int sourceId, SourceType sourceType);
    }
}