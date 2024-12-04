using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Extensions
{
	public static class Validation
	{
		public static bool PhotoIsValid(this IFormFile formFile)
		{
			if (formFile != null)
			{
				string fileExtensionPhoto = Path.GetExtension(formFile.FileName);
				if (fileExtensionPhoto != ".png"
					&& fileExtensionPhoto != ".jpeg"
					&& fileExtensionPhoto != ".jpg")
					return false;
			}
			return true;
		}
	}
}
