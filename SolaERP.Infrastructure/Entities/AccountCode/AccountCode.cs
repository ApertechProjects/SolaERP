using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.AccountCode
{
    public class AccountCode : BaseEntity
    {
        [DbColumn("AccountCode")]
        public string Account_Code { get; set; }
        [DbColumn("AccountName")]
        public string Description { get; set; }
    }
}
