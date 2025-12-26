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
    private readonly int _rfqMainId;
    private readonly IConfiguration _configuration;

    public VM_BidComparisonApprove(string lang, string buyerName, string bidComparisonNo,
        int bidComparisonMainId, int rfqMainId, string businessUnitName)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder =
            new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _buyerName = buyerName;
        _bidComparisonNo = bidComparisonNo;
        _bidComparisonMainId = bidComparisonMainId;
        _rfqMainId = rfqMainId;
        _businessUnitName = businessUnitName;
    }

    public string Subject
    {
        get { return $"{_bidComparisonNo} Bid Comparison Approved / Təkliflərin Müqayisə Cədvəli Təsdiqləndi"; }
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
        string baseUrl = _configuration["Mail:ServerUrlUI"];
        string requestLink = $"{baseUrl}/bid-comparison/{_rfqMainId}?bidComparisonId={_bidComparisonMainId}"; 

        
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_buyerName},</p></td></tr>" +
            $"<tr><td><p>Müqayisə Cədvəli [{_bidComparisonNo}] Kateqoriya Meneceri tərəfindən nəzərdən keçirilmiş və təsdiqlənmişdir.</p></td></tr>" +
            
            $"<tr><td><p>Artıq satınalma prosesinin növbəti mərhələsi üçün hazırdır. Təsdiqlənmiş müqayisə cədvəlini aşağıdakı link vasitəsilə nəzərdən keçirə bilərsiniz:</p></td></tr>" +
            $"<tr><td><p>Müqayisə Cədvəli Nömrəsi: <a href='{requestLink}' style='color: #00008B; text-decoration: underline;'>{_bidComparisonNo}</a></p></td></tr>" +
            $"<tr><td><p>Zəhmət olmasa satınalma iş axınına uyğun olaraq növbəti addımı həyata keçirin.</p></td></tr>" +
            "<tr><td><p>Hörmətlə,<br>SOLA ERP</p></td></tr>" +
            "</table>"
        );
    }

    public HtmlString GetBodyOfMailEn()
    {
        string baseUrl = _configuration["Mail:ServerUrlUI"];
        string requestLink = $"{baseUrl}/bid-comparison/{_rfqMainId}?bidComparisonId={_bidComparisonMainId}"; 
        
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Dear {_buyerName},</p></td></tr>" +
            $"<tr><td><p>The Bid Comparison Table [{_bidComparisonNo}] has been reviewed and approved by the Category Manager." +
            "<tr><td><p> It is now ready for the next step in the procurement process. You can review the approved comparison table via the following link:</p></td></tr>" +
            $"<tr><td><p>Bid Comparison Number: <a href='{requestLink}' style='color: #00008B; text-decoration: underline;'>{_bidComparisonNo}</a></p></td></tr>" +
            
            "<tr><td><p>Please proceed with the next action as per the procurement workflow.</p></td></tr>" +
            "<tr><td><p>Best regards,<br>SOLA ERP</p></td></tr>" +
            "</table>"
        );
    }

}