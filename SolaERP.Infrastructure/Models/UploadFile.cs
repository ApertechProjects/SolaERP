using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SolaERP.Application.Models
{
    public class UploadFile
    {
        public List<IFormFile> Files { get; set; }
        public string Module { get; set; }
        [JsonIgnore]
        public string BearerToken { get; set; }
    }
}
