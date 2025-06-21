using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels
{
    public class VM_BuyerPurchaseOrderApprove : VM_EmailTemplateBase
    {
        private readonly string _buyerName;
        private readonly string _orderNo;
        private readonly int _orderId;
        private readonly string _businessUnitName;
        private readonly IConfiguration _configuration;

        public VM_BuyerPurchaseOrderApprove(string lang, string buyerName, string orderNo, string businessUnitName , int orderId)
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder =
                new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
            _buyerName = buyerName;
            _orderNo = orderNo;
            _orderId = orderId;
            _businessUnitName = businessUnitName;
        }

        public string Subject
        {
            get { return $"Order Approval"; }
        }

        public string TemplateName()
        {
            return @"BuyerPurchaseOrderApprove.cshtml";
        }

        public string GetHeaderOfMailAz
        {
            get { return $"{_orderNo} nömrəli sifariş təsdiqləndi"; }
        }

        public string GetHeaderOfMailEn
        {
            get { return $"Order {_orderNo} Approval Notification"; }
        }

        public HtmlString GetBodyOfMailAz()
        {
            return new HtmlString(
                "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
                $"<tr><td><p>Hörmətli {_buyerName},</p></td></tr>" +
                $"<tr><td><p>Bildirmək istəyirik ki, <b><a href='{_configuration["Mail:ServerUrlUI"] + "/orders/" + _orderId}' style='text-decoration: none;'> {_orderNo} </a></b> nömrəli sifariş təsdiqlənmişdir.</p></td></tr>" +
                $"<tr><td><p>Biznes bölməsi - {_businessUnitName}</p></td></tr>" +
                "<tr><td><p>Sifarişlə bağlı hər hansı sualınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p></td></tr>" +
                "<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
                "</table>"
            );
        }

        public HtmlString GetBodyOfMailEn()
        {
            return new HtmlString(
                "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
                $"<tr><td><p>Dear {_buyerName},</p></td></tr>" +
                $"<tr><td><p>We would like to inform you that order <b><a href='{_configuration["Mail:ServerUrlUI"] + "/orders/" + _orderId}' style='text-decoration: none;'> {_orderNo} </a></b> has been approved.</p></td></tr>" +
                $"<tr><td><p>Business Unit - {_businessUnitName}</p></td></tr>" +
                "<tr><td><p>If you have any questions regarding the order, please feel free to contact us.</p></td></tr>" +
                "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
                "</table>"
            );
        }
    }
}