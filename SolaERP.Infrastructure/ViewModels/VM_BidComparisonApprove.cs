using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_BidComparisonApprove : VM_EmailTemplateBase
{

    private readonly string _buyerName;
    private readonly string _bidComparisonNo;
    private readonly string _businessUnitName;
    private readonly int _bidComparisonMainId;
    private readonly IConfiguration _configuration;

    public VM_BidComparisonApprove(string lang, string buyerName, string bidComparisonNo,
        int bidComparisonMainId, string businessUnitName)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder =
            new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _buyerName = buyerName;
        _bidComparisonNo = bidComparisonNo;
        _bidComparisonMainId = bidComparisonMainId;
        _businessUnitName = businessUnitName;
    }

    public string Subject
    {
        get { return "Bid Comparison Approved"; }
    }

    public string TemplateName()
    {
        return @"BidComparisonForBuyer.cshtml";
    }

    public string GetHeaderOfMailAz
    {
        get { return $" [{_bidComparisonNo}] №-li Təklif Müqayisəsi təsdiqlənmişdir"; }
    }

    public string GetHeaderOfMailEn
    {
        get { return $" [{_bidComparisonNo}] № Bid Comparison approved"; }
    }

    public HtmlString GetBodyOfMailAz()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_buyerName},</p></td></tr>" +
            $"<tr><td><p>[{_bidComparisonNo}] №-li Təklif Müqayisəsi təsdiqlənmişdir.</p></td></tr>" +
            
            $"<tr><td><p>BusinessUnit - {_businessUnitName}</p></td></tr>" +
            "<tr><td><p>Hər hansı bir sualınız və ya köməyə ehtiyacınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p></td></tr>" +
            "<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }

    public HtmlString GetBodyOfMailEn()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Dear {_buyerName},</p></td></tr>" +
            $"<tr><td><p> [{_bidComparisonNo}] № Bid Comparison approved.</p></td></tr>" +
            
            $"<tr><td><p>BusinessUnit - {_businessUnitName}</p></td></tr>" +
            "<tr><td><p>Should you have any questions or require assistance, feel free to contact us.</p></td></tr>" +
            "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }

}