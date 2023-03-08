using SolaERP.Infrastructure.Dtos.Attachment;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IAttachmentService
    {
        Task<ApiResponse<List<AttachmentWithFileDto>>> GetAttachmentWithFilesAsync(int attachmentId);
        Task<ApiResponse<List<AttachmentDto>>> GetAttachmentsAsync(AttachmentListGetModel model);
        Task<ApiResponse<string>> SaveAttachmentAsync(AttachmentSaveModel model);
        Task<ApiResponse<string>> DeleteAttachmentAsync(int attachmentId);

    }
}
