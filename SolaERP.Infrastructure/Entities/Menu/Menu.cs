namespace SolaERP.Application.Entities.Menu
{
    public class Menu : BaseEntity
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string MenuCode { get; set; }
        public string ReactIcon { get; set; }
    }
}
