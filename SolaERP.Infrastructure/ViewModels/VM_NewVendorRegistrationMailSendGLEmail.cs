using Microsoft.AspNetCore.Html;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels
{
	public class VM_NewVendorRegistrationMailSendGLEmail : VM_EmailTemplateBase
	{
		private readonly string _vendorName;

		public VM_NewVendorRegistrationMailSendGLEmail(string lang, string vendorName)
		{
			_vendorName = vendorName;
		}
		
		public string Subject
		{
			get { return "New Vendor Registration"; }
		}

		public string TemplateName()
		{
			return @"NewVendorRegistrationMailSendGLEmail.cshtml";
		}
		
		public string GetHeaderOfMailAz
		{
			get
			{
				return "Yeni Vendor Qeydiyyatı";
			}
		}
    
		public string GetHeaderOfMailEn
		{
			get
			{
				return "New Vendor Registration";
			}
		}

		public HtmlString GetBodyOfMailAz()
		{
			return new HtmlString(
				"<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
				"<tr><td><p>Hörmətli Administrator,</p></td></tr>" +
				"<tr><td><p>Aşağıda qeyd olunan yeni vendor qeydiyyatdan keçmişdir. Xahiş edirik, vendor məlumatlarını nəzərdən keçirdib, təsdiq mərhələsinə göndərin.</p></td></tr>" +
				$"<tr><td><p><b>Vendor Adı: {_vendorName}</b></p></td></tr>" +
				"<tr><td><p><b><a href='http://sola.glcorp.ltd/vendors' style='text-decoration: none;'>Təsdiq üçün daxil olun</a></b></p></td></tr>" +
				"<tr><td><p>Əlavə suallarınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p></td></tr>" +
				"<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
				"</table>"
			);
		}
    
		public HtmlString GetBodyOfMailEn()
		{
			return new HtmlString(
				"<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
				"<tr><td><p>Dear Administrator,</p></td></tr>" +
				"<tr><td><p>The new vendor below has registered. Please review the vendor information and proceed with the approval process.</p></td></tr>" +
				$"<tr><td><p><b>Vendor Name: {_vendorName}</b></p></td></tr>" +
				"<tr><td><p><b><a href='http://sola.glcorp.ltd/vendors' style='text-decoration: none;'>Login for approval</a></b></p></td></tr>" +
				"<tr><td><p>If you have any further questions, please do not hesitate to contact us.</p></td></tr>" +
				"<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
				"</table>"
			);
		}
	}
}
