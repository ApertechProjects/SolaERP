using SolaERP.Application.Dtos.General;
using SolaERP.Application.Entities.BusinessUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Utils
{
	public static class CurrencyOps
	{
		public static async Task<List<ConvRateDto>> GetConvRateDtoAsync(List<ConvRateDto> convDtoList, DateTime date, BusinessUnits businessUnit)
		{
			var resultBase = convDtoList.Where(x =>
				x.EffFromDateTime <= date
				&& x.EffToDateTime >= date
				//&& x.CurrCodeFrom == currency + "  "
				&& x.CurrCodeTo == businessUnit.BaseCurrencyCode + "  "
			);


			return resultBase.ToList();
		}

		public static async Task<ConvRateDto> GetConvRateDtoAsync(List<ConvRateDto> convDtoList, DateTime date,
		 string currency, BusinessUnits businessUnit)
		{
			var singleResultBase = convDtoList.SingleOrDefault(x =>
				x.EffFromDateTime <= date
				&& x.EffToDateTime >= date
				&& x.CurrCodeFrom == currency + "  "
				&& x.CurrCodeTo == businessUnit.BaseCurrencyCode + "  "
			);


			return singleResultBase;
		}
	}
}
