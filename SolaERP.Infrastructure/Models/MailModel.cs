using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class MailModel
    {
        public string Subject { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string ImageName { get; set; }
        public List<string> Tos { get; set; }
    }
}
