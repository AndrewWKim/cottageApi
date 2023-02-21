using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Services
{
	public interface ICommonService
	{
		Task<IEnumerable<ResidentType>> GetResidentTypesAsync();

		Task<decimal> GetCottagesLeftAreaAsync();

		string[] GetCameraIPs(int page);
	}
}
