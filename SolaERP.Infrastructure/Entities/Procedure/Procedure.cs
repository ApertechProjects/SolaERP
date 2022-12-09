namespace SolaERP.Infrastructure.Entities.Procedure
{
    public class Procedure : BaseEntity
    {
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string ProcedureKey { get; set; }
    }
}
