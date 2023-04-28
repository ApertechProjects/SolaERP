using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts
{
    public interface IFileProcessor
    {
        Task<string> UploadAsync(FileModel model, string path, CancellationToken cancellationToken = default);
        Task<string> RenameAsync(string name);
    }
}
