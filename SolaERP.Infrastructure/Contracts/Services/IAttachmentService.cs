using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAttachmentService
    {
        Task<ApiResponse<List<AttachmentWithFileDto>>> GetAttachmentWithFilesAsync(int attachmentId);
        Task<ApiResponse<List<AttachmentDto>>> GetAttachmentsAsync(int sourceId, string reference, string sourceType);
        Task<ApiResponse<List<string>>> GetAttachmentsAsync(int sourceId, int sourceType);
        Task<ApiResponse<string>> SaveAttachmentAsync(AttachmentSaveModel model);
        Task<ApiResponse<string>> DeleteAttachmentAsync(int attachmentId);
        Task<ApiResponse<string>> DeleteAttachmentAsync(int sourceId, SourceType sourceType);

    }
}
