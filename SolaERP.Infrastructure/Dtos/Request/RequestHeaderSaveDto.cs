using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Request
{
    public class RequestHeaderSaveDto
    {
        public int RequestMainId { get; set; }
        public int RequestDetailId { get; set; }
        public int UserId { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public int MyProperty { get; set; }
    }
}
