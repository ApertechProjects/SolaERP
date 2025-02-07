using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolaERP.Application.Dtos.Request;

namespace SolaERP.Application.Models
{
    public class RequestSetBuyer
    {
        public string RequestNo { get; set; }
        public string Buyer { get; set; }
        public int BusinessUnitId { get; set; }
        public List<RequestDetailUpdateBuyerDto> RequestDetails { get; set; }
    }
}
