using SolaERP.Application.Attributes;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class GroupMenuPrivilegeListModel
    {
        public int MenuId { get; set; }
        [NotInclude]
        public bool CreateR { get; set; }
        [NotInclude]
        public bool EditR { get; set; }
        [NotInclude]
        public bool DeleteR { get; set; }
        [NotInclude]
        public bool ExportR { get; set; }
        [JsonIgnore]
        public int Create 
        {
            get
            {
                return Convert.ToInt32(CreateR);
            } 
            set
            {
                Create = value;
            }
        }
        [JsonIgnore]
        public int Edit
        {
            get
            {
                return Convert.ToInt32(EditR);
            }
            set
            {
                Create = value;
            }
        }
        [JsonIgnore]
        public int Delete
        {
            get
            {
                return Convert.ToInt32(DeleteR);
            }
            set
            {
                Create = value;
            }
        }

        [JsonIgnore]
        public int Export
        {
            get
            {
                return Convert.ToInt32(ExportR);
            }
            set
            {
                Create = value;
            }
        }

    }
}
