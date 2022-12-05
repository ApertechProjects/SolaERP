namespace SolaERP.Infrastructure.Entities.Menu
{
    public class Menu : BaseEntity
    {
        public int Id { get; set; }
        public string ParentMenuName { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string ReactIcon { get; set; }
        public List<ChildMenu> ChildMenus { get; set; } = new List<ChildMenu>();
    }
}
