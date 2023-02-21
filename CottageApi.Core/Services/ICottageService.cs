using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Services
{
	public interface ICottageService
	{
		Task<Tuple<IEnumerable<Cottage>, int>> GetCottagesAsync(int? offset = null, int? limit = null, bool? withoutOwners = true);

		Task<Cottage> GetCottageByIdAsync(int id);

		Task<Cottage> CreateCottageAsync(Cottage cottage);

		Task<Cottage> UpdateCottageAsync(Cottage cottage);
	}
}
