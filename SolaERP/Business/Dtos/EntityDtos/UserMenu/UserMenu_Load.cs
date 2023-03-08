namespace SolaERP.Business.Dtos.EntityDtos.UserMenu
{
    public partial class UserMenu_Load
    {
        public System.Int32 MenuId { get; set; }
        public System.String MenuCode { get; set; }
        public System.String MenuName { get; set; }
        public System.Int32 ParentId { get; set; }
        public System.String URL { get; set; }
        public System.String Icon { get; set; }
        public System.String ReactIcon { get; set; }
        public System.Boolean CreateAccess { get; set; }
        public System.Boolean EditAccess { get; set; }
        public System.Boolean DeleteAccess { get; set; }
        public System.Boolean ExportAccess { get; set; }
    }


    public partial class UserMenu_Load
    {
        public System.Boolean ApproveAccess { get; set; }
        public System.Boolean RejectAccess { get; set; }
    }
}
