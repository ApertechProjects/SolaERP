using Newtonsoft.Json;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class DueDiligenceDesignDto
    {
        public string Title { get; set; }
        public List<DueDiligenceChildDto> Childs { get; set; }
    }

    public class DueDiligenceChildDto
    {
        private DateTime? _dateTimeValue;
        public int DesignId { get; set; }
        public int LineNo { get; set; }

        public string Question { get; set; }

        //
        public bool HasTextBox { get; set; }

        //
        public bool HasCheckBox { get; set; }

        //
        public bool HasRadioBox { get; set; }

        //
        public bool HasInt { get; set; }

        //
        public bool HasDecimal { get; set; }

        //
        public bool HasDateTime { get; set; }

        //
        public bool HasAttachment { get; set; }

        //
        public bool HasBankList { get; set; }

        //
        public bool HasTexArea { get; set; }

        //
        public bool ParentCompanies { get; set; }

        //
        public bool HasDataGrid { get; set; }

        //
        public int GridRowLimit { get; set; }

        //
        public int GridColumnCount { get; set; }

        //
        public bool HasAgreement { get; set; }

        //
        public string AgreementText { get; set; }

        //
        public string[] GridColumns { get; set; }

        //
        public decimal TextBoxPoint { get; set; }

        //
        public decimal CheckBoxPoint { get; set; }

        //
        public decimal RadioBoxPoint { get; set; }

        //
        public decimal IntPoint { get; set; }

        //
        public decimal DateTimePoint { get; set; }

        //
        public decimal AttachmentPoint { get; set; }

        //
        public decimal TextAreaPoint { get; set; }

        //
        public decimal BankListPoint { get; set; }

        //
        public decimal DataGridPoint { get; set; }

        //
        public List<DueDiligenceGrid> GridDatas { get; set; }

        //
        public string TextboxValue { get; set; }

        //
        public string BankListValue { get; set; }

        //
        public string TextareaValue { get; set; }

        //
        public bool CheckboxValue { get; set; }

        //
        public bool RadioboxValue { get; set; }

        //
        public int IntValue { get; set; }

        //
        public decimal DecimalValue { get; set; }

        //
        public DateTime? DateTimeValue
        {
            get
            {
                if (_dateTimeValue?.Date == DateTime.MinValue)
                    _dateTimeValue = DateTime.Now;
                return _dateTimeValue;
            }
            set => _dateTimeValue = value;
        }

        //
        public bool AgreementValue { get; set; }

        //
        public decimal Scoring { get; set; }

        //
        public List<AttachmentDto> Attachments { get; set; }

        //
        public decimal DecimalPoint { get; set; }
        public decimal Weight { get; set; }
        public decimal Outcome { get; set; }
        public decimal AllPoint { get; set; }
        public int Type { get; set; }
        public bool Disabled { get; set; }
    }
    
    public class DueDiligenceDesignSaveDto
    {
        public string Title { get; set; }
        public List<DueDiligenceChildSaveDto> Childs { get; set; }
    }

    public class DueDiligenceChildSaveDto
    {
        public int DesignId { get; set; }
        public int LineNo { get; set; }

        public string Question { get; set; }

        //
        public bool HasTextBox { get; set; }

        //
        public bool HasCheckBox { get; set; }

        //
        public bool HasRadioBox { get; set; }

        //
        public bool HasInt { get; set; }

        //
        public bool HasDecimal { get; set; }

        //
        public bool HasDateTime { get; set; }

        //
        public bool HasAttachment { get; set; }

        //
        public bool HasBankList { get; set; }

        //
        public bool HasTexArea { get; set; }

        //
        public bool ParentCompanies { get; set; }

        //
        public bool HasDataGrid { get; set; }

        //
        public int GridRowLimit { get; set; }

        //
        public int GridColumnCount { get; set; }

        //
        public bool HasAgreement { get; set; }

        //
        public string AgreementText { get; set; }

        //
        public string[] GridColumns { get; set; }

        //
        public decimal TextBoxPoint { get; set; }

        //
        public decimal CheckBoxPoint { get; set; }

        //
        public decimal RadioBoxPoint { get; set; }

        //
        public decimal IntPoint { get; set; }

        //
        public decimal DateTimePoint { get; set; }

        //
        public decimal AttachmentPoint { get; set; }

        //
        public decimal TextAreaPoint { get; set; }

        //
        public decimal BankListPoint { get; set; }

        //
        public decimal DataGridPoint { get; set; }

        //
        public List<DueDiligenceGrid> GridDatas { get; set; }

        //
        public string TextboxValue { get; set; }

        //
        public string BankListValue { get; set; }

        //
        public string TextareaValue { get; set; }

        //
        public bool CheckboxValue { get; set; }

        //
        public bool RadioboxValue { get; set; }

        //
        public int IntValue { get; set; }

        //
        public decimal DecimalValue { get; set; }

        //
        public string? DateTimeValue { get; set; }

        //
        public bool AgreementValue { get; set; }
        public decimal Scoring { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
        public decimal DecimalPoint { get; set; }
        public decimal Weight { get; set; }
        public decimal Outcome { get; set; }
        public decimal AllPoint { get; set; }
        public int Type { get; set; }
        public bool Disabled { get; set; }
    }
}