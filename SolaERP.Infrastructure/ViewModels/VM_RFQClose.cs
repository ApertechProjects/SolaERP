using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.ViewModels
{
	public class VM_RFQClose : VM_EmailTemplateBase
	{
		private readonly IConfiguration _configuration;
		private readonly string _lang;
		public VM_RFQClose(string lang)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
			.AddJsonFile(AppSettingsHelper.GetAppSettingsFileName(), optional: true, reloadOnChange: false);

			_configuration = builder.Build();
			_lang = lang;
		}

		public string? Token { get; set; }
		public string? Subject
		{
			get
			{
				return _lang switch
				{
					"az" => "RFQ üçün təqdim etmə müddəti",
					"en" => "RFQ period",
					_ => "en"
				};
			}
		}
		public string? Username { get; set; }


		public string TemplateName()
		{
			return @"RFQClose.cshtml";
		}

		public string GetHeaderOfMail()
		{
			switch (_lang)
			{
				case "az":
					return "RFQ üçün təqdim etmə müddəti";
				case "en":
					return "RFQ period";
			}
			return "";
		}
		public HtmlString GetBodyOfMail(string vendorName)
		{
			switch (_lang)
			{
				case "az":
					return new HtmlString($"Hörmətli {vendorName}, <br> " +
						"Sizə bildiririk ki, RFQ üçün təqdimetmə müddəti rəsmi olaraq başa çatmışdır. Təklifiniz növbəti baxış mərhələsinə göndərilmişdir. <br> Xahiş edirik təsdiq prosesinin nəticələrini gözləyin. Əlavə məlumat və ya aydınlaşdırma tələb olunarsa, sizinlə əlaqə saxlayacağıq. <br>" +
						"İştirakınız və əməkdaşlığınız üçün təşəkkür edirik.");
				case "en":
					return new HtmlString($"Dear {vendorName}, <br> " +
						"We would like to inform you that the submission period for the RFQ has officially closed. Your proposal has been successfully forwarded to the next stage of review.<br> " +
						"Please await the results of this process. Should any additional information or clarification be required, we will reach out to you promptly. <br> Thank you for your participation and cooperation.");
			}
			return new HtmlString("");
		}


	}
}
