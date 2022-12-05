namespace SolaERP.Infrastructure.Entities.Menu
{
    public class MenuWithPrivilages : BaseEntity
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string MenuCode { get; set; }
        public bool CreateAccess { get; set; }
        public bool EditAccess { get; set; }
        public bool DeleteAccess { get; set; }
        public bool ExportAccess { get; set; }
        public string ReactIcon { get; set; }
    }
}

