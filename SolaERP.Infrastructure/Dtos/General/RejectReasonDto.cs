using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.General
{
    public class RejectReasonDto
    {
        public int RejectReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
        public bool BackToInitiator { get; set; }
    }
}
