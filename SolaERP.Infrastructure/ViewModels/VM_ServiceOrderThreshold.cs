using Microsoft.AspNetCore.Html;
using SolaERP.Application.Enums;

namespace SolaERP.Application.ViewModels;

public class VM_ServiceOrderThreshold
{
    public HtmlString Body { get; set; }
    public string FullName { get; set; }
    public string Header { get; set; }
    public string Subject { get; set; }
    public string ServiceOrderNo { get; set; }
    public decimal Percentage { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string Currency { get; set; }
    public string Url { get; set; }
    public EmailTemplateKey TemplateKey { get; set; }

    public string TemplateName()
    {
        return "ServiceOrderThreshold";
    }
}