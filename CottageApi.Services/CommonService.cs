using CottageApi.Core.Configurations;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CottageApi.Services
{
	public class CommonService : ICommonService
	{
		private readonly ICottageDbContext _cottageDbContext;
		private readonly Config _config;

		public CommonService(
			ICottageDbContext cottageDbContext,
			Config config)
		{
			_cottageDbContext = cottageDbContext;
			_config = config;
		}

		public async Task<IEnumerable<ResidentType>> GetResidentTypesAsync()
		{

			var residentTypes = await _cottageDbContext.ResidentTypes.ToListAsync();

			return residentTypes;
		}

		public async Task<decimal> GetCottagesLeftAreaAsync()
		{
			var fullArea = _config.CottageVillageArea;

			var cottages = await _cottageDbContext.Cottages.ToListAsync();
			var cottagesSumArea = cottages.Sum(c => c.Area);

			return fullArea - cottagesSumArea;
		}

		public string[] GetCameraIPs(int page)
		{
			var ips = _config.CameraIPs;
			if (page == 4)
			{
				ips = ips.Skip(30).Take(8).ToArray();
				return ips;
			}

			ips = ips.Skip((page -1) * 10).Take(10).ToArray();
			return ips;
		}
	}
}
