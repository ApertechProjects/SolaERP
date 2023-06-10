using Newtonsoft.Json;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class VendorDueDiligenceDto
    {
        public int Id { get; set; }
        public int DesignId { get; set; }
        public int VendorId { get; set; }
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
        public decimal Scoring { get; set; }
    }
}
