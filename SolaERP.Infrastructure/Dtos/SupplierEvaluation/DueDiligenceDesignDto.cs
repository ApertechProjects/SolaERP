using Newtonsoft.Json;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class DueDiligenceDesignDto
    {
        public string Title { get; set; }
        public List<DueDiligenceChildDto> Childs { get; set; }

    }

    public class DueDiligenceChildDto
    {
        public int DesignId { get; set; }
        public int LineNo { get; set; }
        public string Question { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasTextBox { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasCheckBox { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasRadioBox { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasInt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasDecimal { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasDateTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasAttachment { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasBankList { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasTexArea { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ParentCompanies { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasDataGrid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? GridRowLimit { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? GridColumnCount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasAgreement { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AgreementText { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] GridColumns { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TextBoxPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CheckBoxPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? RadioBoxPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? IntPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DateTimePoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? AttachmentPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TextAreaPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? BankListPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DataGridPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DueDiligenceGrid> GridDatas { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextboxValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextareaValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? CheckboxValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? RadioboxValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? IntValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DecimalValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateTimeValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? AgreementValue { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Scoring { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AttachmentDto> Attachments { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DecimalPoint { get; set; }
    }

}
