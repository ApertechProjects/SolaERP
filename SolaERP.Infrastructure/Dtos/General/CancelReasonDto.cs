namespace SolaERP.Application.Dtos.General
{
    public class CancelReasonDto
    {
        public int CancelReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
        public bool BackToInitiator { get; set; }
    }
}
