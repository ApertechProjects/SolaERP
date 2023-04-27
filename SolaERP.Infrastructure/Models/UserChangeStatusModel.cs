namespace SolaERP.Application.Models
{
    public class UserChangeStatusModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
    }
}
