using System.Threading.Tasks;
using CottageApi.Controllers.Base;
using CottageApi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
	[Route("api/[controller]")]
	public class CommonController : BaseApiController
	{
		private readonly ICommonService _commonService;

		public CommonController(
			ICommonService commonService)
		{
			_commonService = commonService;
		}

		[AllowAnonymous]
		[HttpGet("resident-types")]
		public async Task<object> GetResidentTypes()
		{
			var residentTypes = await _commonService.GetResidentTypesAsync();
			return residentTypes;
		}

		[HttpGet("left-area")]
		public async Task<decimal> GetCottagesLeftArea()
		{
			var leftAreaNumber = await _commonService.GetCottagesLeftAreaAsync();
			return leftAreaNumber;
		}

		[HttpGet("camera-ips/{page}")]
		public string[] GetCameraIPs([FromRoute]int page)
		{
			var ips = _commonService.GetCameraIPs(page);
			return ips;
		}

		/*[AllowAnonymous]
		[HttpGet("hash")]
		public string Hash()
		{
			var header = EncryptionHelper.HashWithBASE64URL("\\{\"alg\":\"RS256\"\\}");
			var payload = EncryptionHelper.HashWithBASE64URL("1756319;E7884158;20210114104000;newsidetest2;980;40000;;");
			var payload = EncryptionHelper.HashWithBASE64URL("\\{\"merchantId\":\"1756319\",\"terminalId\":\"E7884158\",\"totalAmount\":50000,\"currency\":\"980\",\"orderId\":\"newsidetest1\",\"purchaseTime\":\"20210114104000\"\\}");
			var signature = header + "." + payload;

			var result = PemSignature.SignStringWithPrivatePem("1756319;E7884158;20210114104039;newsidetest93;980;93000;;");
			result = "5678910_5000_3_";
			result = result.Substring(result.IndexOf("_") + 1);
			return result;
		}*/
	}
}
