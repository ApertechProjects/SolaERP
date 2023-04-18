namespace SolaERP.Infrastructure.Dtos.Menu
{
    public class ParentMenuDto
    {
        public int Id { get; set; }
        public string ParentMenuName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string ReactIcon { get; set; }
        public bool ReadAccess { get; set; }
        public List<ChildMenuDto> Childs { get; set; } = new List<ChildMenuDto>();
    }
}
