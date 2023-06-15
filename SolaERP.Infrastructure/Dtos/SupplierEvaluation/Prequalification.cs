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

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasTextbox { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasTextarea { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasCheckbox { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasRadiobox { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasInt { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasDecimal { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasDateTime { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasAttachment { get; set; }

        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public bool  HasList { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasGrid { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int GridRowLimit { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int GridColumnCount { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] GridColumns { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal TextboxPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal TextareaPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CheckboxPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal RadioboxPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal IntPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DecimalPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DateTimePoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Attachmentpoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ListPoint { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextboxValue { get; set; }
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextareaValue { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool CheckboxValue { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool RadioboxValue { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int IntValue { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal DecimalValue { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateTimeValue { get; set; }

        // //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Scoring { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AttachmentDto> Attachments { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PrequalificationGridData> GridDatas { get; set; }
        public decimal Weight { get; set; }
        public decimal Outcome { get; set; }

    }
}
