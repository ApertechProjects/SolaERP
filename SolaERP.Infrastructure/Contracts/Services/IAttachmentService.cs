using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IAttachmentService
    {
        Task<ApiResponse<List<AttachmentWithFileDto>>> GetAttachmentWithFilesAsync(int attachmentId);
        Task<ApiResponse<List<AttachmentDto>>> GetAttachmentsAsync(AttachmentListGetModel model);
        Task<ApiResponse<string>> SaveAttachmentAsync(AttachmentSaveModel model);
        Task<ApiResponse<string>> DeleteAttachmentAsync(int attachmentId);

    }
}
