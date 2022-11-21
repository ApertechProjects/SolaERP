using SolaERP.Business.Dtos.GeneralDtos;

namespace SolaERP.Business.Dtos.Wrappers
{
    public class SaveFileWrapper
    {
        public SaveFileWrapper()
        {
            SavedFiles = new();
            DeletedFileIds = new();
        }
        public List<AppAttachment> SavedFiles { get; set; }
        public List<int> DeletedFileIds { get; set; }
    }
}
