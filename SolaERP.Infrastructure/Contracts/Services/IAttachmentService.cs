using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAttachmentService
    {
        Task<List<AttachmentDto>> GetAttachmentsAsync(int sourceId, SourceType sourceType, Modules module,
            int attachmentTypeId = 0, string reference = null, bool isDownloadLink = true);

        Task<AttachmentDto> GetAttachmentById(int attachmentId, bool getLink = false,
            Modules module = default, bool isDownloadLink = true);

        Task SaveAttachmentAsync(AttachmentSaveModel model, SourceType sourceType, int sourceId);
        Task SaveAttachmentAsync(List<AttachmentSaveModel> attachments, SourceType sourceType, int sourceId);
        Task<bool> DeleteAttachmentAsync(int attachmentId);
        Task<bool> DeleteAttachmentAsync(int sourceId, SourceType sourceType);
    }
}