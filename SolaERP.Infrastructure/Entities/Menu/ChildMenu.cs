namespace SolaERP.Infrastructure.Entities.Menu
{
    public class ChildMenu : BaseEntity
    {
        public int Id { get; set; }
        public string SubMenuName { get; set; }
        public string Icon { get; set; }
        public string SubLink { get; set; }
        public string ReactIcon { get; set; }
        public int ParentMenuId { get; set; }
        public Menu ParentMenu { get; set; }
    }
}
