namespace SolaERP.Application.Entities.General
{
    public class CancelReason : BaseEntity
    {
        public int CancelReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
        public bool? BackToInitiator { get; set; }
    }
}
