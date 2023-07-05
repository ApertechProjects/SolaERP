using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class GroupMenuPrivilegeListModel
    {
        public int MenuId { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Export { get; set; }
        [JsonIgnore]
        public int CreateInt { get; set; }
        [JsonIgnore]
        public int EditInt { get; set; }
        [JsonIgnore]
        public int DeleteInt { get; set; }
        [JsonIgnore]
        public int ExportInt { get; set; }
    }
}
