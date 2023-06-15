using Newtonsoft.Json;
using SolaERP.Application.Dtos.Attachment;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class PrequalificationWithCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<VM_GET_Prequalification> Prequalifications { get; set; }
    }

    public class VM_GET_Prequalification
    {
        public string Title { get; set; }
        public List<PrequalificationDto> Childs { get; set; }
    }

    public class PrequalificationDto
    {
        public int DesignId { get; set; }
        public int LineNo { get; set; }
        public string Discipline { get; set; }
        public string Questions { get; set; }
        public bool HasTextbox { get; set; }
        public bool HasTextarea { get; set; }
        public bool HasCheckbox { get; set; }
        public bool HasRadiobox { get; set; }
        public bool HasInt { get; set; }
        public bool HasDecimal { get; set; }
        public bool HasDateTime { get; set; }
        public bool HasAttachment { get; set; }
        public string Title { get; set; }
        public bool HasGrid { get; set; }
        public int GridRowLimit { get; set; }
        public int GridColumnCount { get; set; }
        public string[] GridColumns { get; set; }
        public decimal TextboxPoint { get; set; }
        public decimal TextareaPoint { get; set; }
        public decimal CheckboxPoint { get; set; }
        public decimal DataGridPoint { get; set; }
        public decimal RadioboxPoint { get; set; }
        public decimal IntPoint { get; set; }
        public decimal DecimalPoint { get; set; }
        public decimal DateTimePoint { get; set; }
        public decimal Attachmentpoint { get; set; }
        public decimal ListPoint { get; set; }
        public string TextboxValue { get; set; }
        public string TextareaValue { get; set; }
        public bool CheckboxValue { get; set; }
        public bool RadioboxValue { get; set; }
        public int IntValue { get; set; }
        public decimal DecimalValue { get; set; }

        public DateTime DateTimeValue { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
        public List<PrequalificationGridData> GridDatas { get; set; }
        public decimal Weight { get; set; }
        public decimal Outcome { get; set; }
        public decimal Scoring { get; set; }
    }
}
