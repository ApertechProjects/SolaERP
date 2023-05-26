namespace SolaERP.Application.Models
{
    public class DueDiligenceGridModel
    {
        public int? DueDesignId { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
    }

    public class DueDiligenceGridUpdateModel
    {
        public int Id { get; set; }
        public int? DueDesignId { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
    }

}
