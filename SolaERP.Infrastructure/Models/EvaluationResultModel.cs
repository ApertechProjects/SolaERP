using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class EvaluationResultModel
    {
        public int VendorId { get; set; }
        public VM_GET_VendorBankDetails BankDetails { get; set; }
    }
}
