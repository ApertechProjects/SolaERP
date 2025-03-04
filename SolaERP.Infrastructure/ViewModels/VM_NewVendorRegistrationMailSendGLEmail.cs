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
				$"<div style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5;'>"+
				"<p>Hörmətli Administrator,</p>" +
				"<p>Aşağıda qeyd olunan yeni vendor qeydiyyatdan keçmişdir. Xahiş edirik, vendor məlumatlarını nəzərdən keçirdib, təsdiq mərhələsinə göndərin.</p>" +
				$"<p>Vendor Adı: {_vendorName}</p>" +
				"<p><b><a href='http://sola.glcorp.ltd/vendors' style='text-decoration: none;'>Təsdiq üçün daxil olun</a></b></p>" +
				"<p>Əlavə suallarınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p>" +
				"<p>Hörmətlə,<br>GL Group</p>"
			);
		}
    
		public HtmlString GetBodyOfMailEn()
		{
			return new HtmlString(
				"<p>Dear Administrator,</p>" +
				"<p>The new vendor below, has registered Please review the vendor information and proceeding with the approval process.</p>" +
				$"<p>Vendor Name: {_vendorName}</p>" +
				"<p><b><a href='http://sola.glcorp.ltd/vendors' style='text-decoration: none;'>Login for approval</a></b></p>" +
				"<p>If you have any further questions, please do not hesitate to contact us.</p>" +
				"<p>Best regards,<br>GL Group</p>" +
				"</div>"
			);
		}
	}
}
