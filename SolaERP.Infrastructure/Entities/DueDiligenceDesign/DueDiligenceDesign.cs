using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.DueDiligenceDesign
{
    public class DueDiligenceDesign : BaseEntity
    {
        public int DueDiligenceDesignId { get; set; }
        public int LineNo { get; set; }
        public string Label { get; set; }
        public bool HasTextbox { get; set; }
        public bool HasCheckbox { get; set; }
        public bool HasRadiobox { get; set; }
        public bool HasInt { get; set; }
        public bool HasDecimal { get; set; }
        public bool HasDateTime { get; set; }
        public bool HasAttachment { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public bool HasBankList { get; set; }
        public bool HasTextarea { get; set; }
        public bool ParentCompanies { get; set; }
        public bool HasGrid { get; set; }
        public int GridRowLimit { get; set; }
        public int GridColumnCount { get; set; }
        public bool HasAgreement { get; set; }
        public string AgreementText { get; set; }

    }
}
