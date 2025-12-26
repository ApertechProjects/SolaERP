using SolaERP.Application.Models;

namespace SolaERP.Application.Dtos.Invoice;

public class InvoiceIUDIntegrationResultModel : SaveResultModel
{
    public int JournalNo { get; set; }
}