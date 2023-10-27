using SolaERP.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Payment
{
    public class ASalfldgAndAllocationDto : BaseEntity
    {
        public string ACCNT_CODE { get; set; }
        public int PERIOD { get; set; }
        public DateTime? TRANS_DATETIME { get; set; }
        public int JRNAL_NO { get; set; }
        public long JRNAL_LINE { get; set; }
        public decimal AMOUNT { get; set; }
        public string D_C { get; set; }
        public string ALLOCATION { get; set; }
        public string JRNAL_TYPE { get; set; }
        public string JRNAL_SRCE { get; set; }
        public string TREFERENCE { get; set; }
        public string DESCRIPTN { get; set; }
        public DateTime? ENTRY_DATETIME { get; set; }
        public int ENTRY_PRD { get; set; }
        public DateTime? DUE_DATETIME { get; set; }
        public int ALLOC_REF { get; set; }
        public DateTime? ALLOC_DATETIME { get; set; }
        public int ALLOC_PERIOD { get; set; }
        public string ASSET_IND { get; set; }
        public string ASSET_CODE { get; set; }
        public string ASSET_SUB { get; set; }
        public string CONV_CODE { get; set; }
        public decimal CONV_RATE { get; set; }
        public decimal OTHER_AMT { get; set; }
        public string OTHER_DP { get; set; }
        public string CLEARDOWN { get; set; }
        public string REVERSAL { get; set; }
        public string LOSS_GAIN { get; set; }
        public string ROUGH_FLAG { get; set; }
        public string IN_USE_FLAG { get; set; }
        public string ANAL_T0 { get; set; }
        public string ANAL_T1 { get; set; }
        public string ANAL_T2 { get; set; }
        public string ANAL_T3 { get; set; }
        public string ANAL_T4 { get; set; }
        public string ANAL_T5 { get; set; }
        public string ANAL_T6 { get; set; }
        public string ANAL_T7 { get; set; }
        public string ANAL_T8 { get; set; }
        public string ANAL_T9 { get; set; }
        public DateTime? POSTING_DATETIME { get; set; }
        public string ALLOC_IN_PROGRESS { get; set; }
        public int HOLD_REF { get; set; }
        public string HOLD_OP_ID { get; set; }
        public decimal BASE_RATE { get; set; }
        public string BASE_OPERATOR { get; set; }
        public string CONV_OPERATOR { get; set; }
        public decimal REPORT_RATE { get; set; }
        public string REPORT_OPERATOR { get; set; }
        public decimal REPORT_AMT { get; set; }
        public decimal MEMO_AMT { get; set; }
        public string EXCLUDE_BAL { get; set; }
        public string LE_DETAILS_IND { get; set; }
        public int CONSUMED_BDGT_ID { get; set; }
        public string CV4_CONV_CODE { get; set; }
        public decimal CV4_AMT { get; set; }
        public decimal CV4_CONV_RATE { get; set; }
        public string CV4_OPERATOR { get; set; }
        public string CV4_DP { get; set; }
        public string CV5_CONV_CODE { get; set; }
        public decimal CV5_AMT { get; set; }
        public decimal CV5_CONV_RATE { get; set; }
        public string CV5_OPERATOR { get; set; }
        public string CV5_DP { get; set; }
        public string LINK_REF_1 { get; set; }
        public string LINK_REF_2 { get; set; }
        public string LINK_REF_3 { get; set; }
        public string ALLOCN_CODE { get; set; }
        public int ALLOCN_STMNTS { get; set; }
        public string OPR_CODE { get; set; }
        public int SPLIT_ORIG_LINE { get; set; }
        public DateTime? VAL_DATETIME { get; set; }
        public string SIGNING_DETAILS { get; set; }
        public DateTime? INSTLMT_DATETIME { get; set; }
        public int PRINCIPAL_REQD { get; set; }
        public string BINDER_STATUS { get; set; }
        public int AGREED_STATUS { get; set; }
        public int SPLIT_LINK_REF { get; set; }
        public string PSTG_REF { get; set; }
        public int TRUE_RATED { get; set; }
        public DateTime? HOLD_DATETIME { get; set; }
        public string HOLD_TEXT { get; set; }
        public int INSTLMT_NUM { get; set; }
        public int SUPPLMNTRY_EXTSN { get; set; }
        public int APRVLS_EXTSN { get; set; }
        public int REVAL_LINK_REF { get; set; }
        public decimal SAVED_SET_NUM { get; set; }
        public int AUTHORISTN_SET_REF { get; set; }
        public int PYMT_AUTHORISTN_SET_REF { get; set; }
        public int MAN_PAY_OVER { get; set; }
        public string PYMT_STAMP { get; set; }
        public int AUTHORISTN_IN_PROGRESS { get; set; }
        public int SPLIT_IN_PROGRESS { get; set; }
        public string VCHR_NUM { get; set; }
        public string JNL_CLASS_CODE { get; set; }
        public string ORIGINATOR_ID { get; set; }
        public DateTime? ORIGINATED_DATETIME { get; set; }
        public string LAST_CHANGE_USER_ID { get; set; }
        public DateTime? LAST_CHANGE_DATETIME { get; set; }
        public string AFTER_PSTG_ID { get; set; }
        public DateTime? AFTER_PSTG_DATETIME { get; set; }
        public string POSTER_ID { get; set; }
        public string ALLOC_ID { get; set; }
        public int JNL_REVERSAL_TYPE { get; set; }
    }
}
