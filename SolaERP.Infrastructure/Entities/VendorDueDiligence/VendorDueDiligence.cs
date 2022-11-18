using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.VendorDueDiligence
{
    public class VendorDueDiligence : BaseEntity
    {
        public int VendorDueDiligenceId { get; set; }
        public int DueDiligenceDesignId { get; set; }
        public int VendorId { get; set; }
        public string TextboxValue { get; set; }
        public string TextareaValue { get; set; }
        public bool CheckboxValue { get; set; }
        public bool RadioboxValue { get; set; }
        public int IntValue { get; set; }
        public decimal decimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public decimal Scoring { get; set; }
        public bool AgreementValue { get; set; }

    }
}
