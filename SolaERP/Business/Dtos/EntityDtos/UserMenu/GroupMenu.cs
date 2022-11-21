namespace SolaERP.Business.Dtos.EntityDtos.UserMenu
{
    public class GroupMenu
    {
        public System.Int32 GroupMenuId { get; set; }
        public System.Int32 MenuId { get; set; }
        public System.Int32 ParentId { get; set; }
        public System.String MenuName { get; set; }
        public System.Int32 CreateAccess { get; set; }
        public System.Int32 EditAccess { get; set; }
        public System.Int32 DeleteAccess { get; set; }
        public System.Int32 ExportAccess { get; set; }
    }
}
