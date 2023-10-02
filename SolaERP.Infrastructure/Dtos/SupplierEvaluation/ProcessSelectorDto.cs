namespace SolaERP.Application.Dtos.SupplierEvaluation;

public class ProcessSelectorDto
{
    public bool IsRevise { get; set; } = false;
    public bool IsUpdate { get; set; } = false;
    public bool IsCreate { get; set; } = false;
}