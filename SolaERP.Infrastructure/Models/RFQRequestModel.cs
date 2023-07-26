using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class RFQRequestModel
    {
        public int BusinessUnitId { get; set; }
        public List<int> BusinessCategoryIds { get; set; }
        public string Buyer { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
