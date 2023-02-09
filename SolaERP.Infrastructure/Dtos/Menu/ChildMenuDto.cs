namespace SolaERP.Infrastructure.Dtos.Menu
{
    public class ChildMenuDto
    {
        public int Id { get; set; }
        public string ChildMenuName { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string ReactIcon { get; set; }
        public int ParentMenuId { get; set; }
        public ParentMenuDto ParentMenu { get; set; }
    }
}
