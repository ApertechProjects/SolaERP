namespace SolaERP.Business.Dtos.EntityDtos.UserMenu
{
    public class ParentMenu
    {
        public ParentMenu()
        {
            SubMenus = new HashSet<SubMenuDto>();
        }
        public int Id { get; set; }
        public string ParentMenuName { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string ReactIcon { get; set; }

        public ICollection<SubMenuDto> SubMenus { get; set; }
    }

    public class SubMenuDto
    {
        public int Id { get; set; }
        public string SubMenuName { get; set; }
        public string Icon { get; set; }
        public string SubLink { get; set; }
        public string ReactIcon { get; set; }
        public int ParentMenuId { get; set; }
        public ParentMenu ParentMenu { get; set; }
    }
}
