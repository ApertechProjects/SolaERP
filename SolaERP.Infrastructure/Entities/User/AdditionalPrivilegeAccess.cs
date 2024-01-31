namespace SolaERP.Application.Entities.User
{
    public class AdditionalPrivilegeAccess : BaseEntity
    {
        //public int GroupAdditionalPrivilegeId { get; set; }
        public int GroupId { get; set; }
        public int AdditionalPrivelegeId { get; set; }
        public string AdditionalPrivelegeName { get; set; }
    }
}
