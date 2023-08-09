using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class UploadFile
    {
        public string message { get; set; }
        public string[] data { get; set; }
    }
}
