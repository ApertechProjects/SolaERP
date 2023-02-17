namespace SolaERP.Infrastructure.Models
{
    public class GroupMenuIDSaveModel
    {
        public int MenuId { get; set; }
        public int GroupId { get; set; }
        public int Create { get; set; } = new();
        public int Edit { get; set; } = new();
        public int Delete { get; set; } = new();
        public int Export { get; set; } = new();

    }
}
