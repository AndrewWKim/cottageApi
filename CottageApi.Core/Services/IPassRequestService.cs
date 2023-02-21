using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Services
{
	public interface IPassRequestService
	{
		Task<Tuple<IEnumerable<PassRequest>, int>> GetPassRequests(DateTime? visitDate = null, int ? cottageId = null, int? offset = null, int? limit = null);

		Task<IEnumerable<PassRequest>> GetPassRequestsForMobile(int cottageId);

		Task DeletePassRequest(int passRequestId);

		Task<PassRequest> CreatePassRequest(PassRequest passRequest);
	}
}
