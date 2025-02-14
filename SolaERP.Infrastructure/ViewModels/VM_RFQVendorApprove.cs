using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_RFQVendorApprove : VM_EmailTemplateBase
{
    private readonly IConfiguration _configuration;
    private readonly string _lang;
    private readonly string _subject;
    private readonly string _body;
    private readonly string _companyName;
    private readonly string _userEmail;
    
    public VM_RFQVendorApprove(string lang, string userEmail ,string companyName , int rfqId)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _lang = lang;
        _userEmail = userEmail;
        _companyName = companyName;
        
    }

    public string? Subject
    {
        get
        {
            return _lang switch
            {
                "az" => "Yeni RFQ sorğusu göndərildi",
                "en" => "A new RFQ request has been sent",
                _ => "en"
            };
        }
    }

    public string TemplateName()
    {
        return @"RFQVendorApprove.cshtml";
    }

    public string GetHeaderOfMail()
    {
        switch (_lang)
        {
            case "az":
                return "Yeni RFQ sorğusu göndərildi";
            case "en":
                return "A new RFQ request has been sent";
        }
        return "";
    }
    
    public string companyName {get;set;}
    public HtmlString GetBodyOfMail()
    {
        switch (_lang)
        {
            case "en":
                return new HtmlString($"Dear {companyName}, <br>"+
                                      "A new RFQ (Request for Quotation) has been sent to you. Please click the link below to review the request and submit your response relatively: <br>"+
                                      $"<b><a href={_configuration["Mail:VendorServerUrlUI"]+"/rfq-information"}>SolaERP</a></b> <br>"+
                                      "If you have any further questions, please do not hesitate to contact us. <br>"+
                                      "Best regards, <br>"+
                                      "GL Group"
                );
            case "az":
                return new HtmlString($"Hörmətli {companyName}, <br>"+
                                      "Sizə yeni RFQ (Təklif Sorğusu) göndərilmişdir. Zəhmət olmasa, aşağıdakı linkə daxil olaraq sorğunu nəzərdən keçirin və müvafiq cavabınızı təqdim edin: <br>"+
                                      $"<b><a href='{_configuration["Mail:VendorServerUrlUI"]+"/rfq-information"}'>SolaERP</a></b> <br>"+
                                      "Əlavə suallarınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin. <br>"+
                                      "Hörmətlə, <br>"+
                                      "GL Group"
                                      );
        }
        return new HtmlString("");
    }
}