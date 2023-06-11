using Newtonsoft.Json;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class PrequalificationWithCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PrequalificationDto> Prequalifications { get; set; }
    }

    public class VM_GET_Prequalification
    {
        public string Title { get; set; }
        public List<PrequalificationDto> Childs { get; set; }
    }

    public class PrequalificationDto
    {
        public int LineNo { get; set; }
        public string Discipline { get; set; }
        public string Questions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasTextbox { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasTextarea { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasCheckbox { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasRadiobox { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasInt { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasDecimal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasDateTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasAttachment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasList { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Weight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasGrid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? GridRowLimit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? GridColumnCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column1Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column2Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column3Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column4Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column5Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TextboxPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TextareaPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CheckboxPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? RadioboxPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? IntPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DecimalPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DateTimePoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Attachmentpoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ListPoint { get; set; }

    }
}
