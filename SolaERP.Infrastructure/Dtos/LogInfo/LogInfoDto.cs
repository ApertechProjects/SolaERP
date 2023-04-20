namespace SolaERP.Infrastructure.Dtos.LogInfo
{
    public class LogInfoDto
    {
        public int LogId { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string BusnessUnitName { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public int SourceId { get; set; }
        public string LogInformation { get; set; }
    }
}
