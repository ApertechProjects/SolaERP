using SolaERP.Application.Attributes;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class GroupMenuPrivilegeListModel
    {
        public int MenuId { get; set; }
        [NotInclude]
        public bool Create { get; set; }
        [NotInclude]
        public bool Edit { get; set; }
        [NotInclude]
        public bool Delete { get; set; }
        [NotInclude]
        public bool Export { get; set; }
        [JsonIgnore]
        public int CreateResult 
        {
            get
            {
                return Convert.ToInt32(Create);
            } 
            set
            {
                CreateResult = value;
            }
        }
        [JsonIgnore]
        public int EditResult
        {
            get
            {
                return Convert.ToInt32(Edit);
            }
            set
            {
                EditResult = value;
            }
        }
        [JsonIgnore]
        public int DeleteResult
        {
            get
            {
                return Convert.ToInt32(Delete);
            }
            set
            {
                DeleteResult = value;
            }
        }

        [JsonIgnore]
        public int ExportResult
        {
            get
            {
                return Convert.ToInt32(Export);
            }
            set
            {
                ExportResult = value;
            }
        }

    }
}
