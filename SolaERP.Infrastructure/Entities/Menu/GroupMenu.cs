namespace SolaERP.Application.Entities.Menu
{
    public class GroupMenu : BaseEntity
    {
        public int GroupMenuId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public int CreateAccess { get; set; }
        public int EditAccess { get; set; }
        public int DeleteAccess { get; set; }
        public int ExportAccess { get; set; }
    }
}
