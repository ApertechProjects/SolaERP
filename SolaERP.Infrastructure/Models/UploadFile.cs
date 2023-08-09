using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class UploadFile
    {
        public List<IFormFile> Files { get; set; }
        public string Module { get; set; }
        public string BearerToken { get; set; }
    }
}
