using Newtonsoft.Json;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class PrequalificationGridData
    {
        public int Id { get; set; }
        public int DesignId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column4 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Column5 { get; set; }

        [JsonIgnore]
        public int VendorId { get; set; }
    }
}
