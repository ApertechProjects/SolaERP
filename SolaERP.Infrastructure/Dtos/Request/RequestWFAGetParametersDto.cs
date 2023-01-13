using SolaERP.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestWFAGetParametersDto
    {
        public int BusinessUnitId { get; set; }
        public string ItemCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int UserId { get; set; }
    }
}
