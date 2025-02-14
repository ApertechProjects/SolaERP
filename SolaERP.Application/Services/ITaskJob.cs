namespace SolaERP.Persistence.Services
{
    public interface ITaskJob
    {
        Queue<string> Queue { get; set; }
    }
}