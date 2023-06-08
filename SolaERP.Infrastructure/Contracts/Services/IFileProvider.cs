using SolaERP.Application.Dtos.Attachment;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileProvider
    {
        Task<string> AddFileAsync(List<AttachmentWithFileDto> files);
        Task<List<AttachmentWithFileDto>> GetFilesAsync(List<string> path);
        Task<(string DeletedFilePath, bool isDeleted)> DeleteFileAsync(string path);
    }
}
