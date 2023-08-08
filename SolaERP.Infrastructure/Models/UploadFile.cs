using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class UploadFile
    {
        public List<IFormFile> Files { get; set; }
        public string Module { get; set; }
    }
}
