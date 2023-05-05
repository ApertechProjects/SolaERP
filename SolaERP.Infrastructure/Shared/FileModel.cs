using SolaERP.Application.Contracts.Repositories;

namespace SolaERP.Application.Shared
{
    public class FileModel
    {
        public string Email { get; set; }
        public byte[] FormDatas { get; set; }
        public string fileName { get; set; }
        public string Path { get; set; }
        public Filetype Type { get; set; }

    }
}
