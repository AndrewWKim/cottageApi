using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Core.Extensions;
using CottageApi.Data.Context;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CottageApi.Core.Enums;
using CottageApi.Core.Exceptions;

namespace CottageApi.Services
{
	public class CottageService : ICottageService
	{

		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		private readonly ICottageDbContext _cottageDbContext;

		public CottageService(
			IMapper mapper,
			ILogger<CottageService> logger,
			ICottageDbContext cottageDbContext)
		{
			_mapper = mapper;
			_logger = logger;
			_cottageDbContext = cottageDbContext;
		}

		public async Task<Tuple<IEnumerable<Cottage>, int>> GetCottagesAsync(int? offset = null, int? limit = null, bool? withoutOwners = false)
		{

			IQueryable<Cottage> query = _cottageDbContext.Cottages.Include(c => c.Clients);

			query = withoutOwners.Value ? query.Where(c => c.Clients.FirstOrDefault(client => client.ClientType == ClientType.Owner) == null) : query;

			int total = await query.CountAsync();

			var cottages = await query
				.OrderBy(x => x.Id)
				.Paging(offset, limit)
				.ToListAsync();

			return new Tuple<IEnumerable<Cottage>, int>(cottages, total);
		}

		public async Task<Cottage> GetCottageByIdAsync(int id)
		{
			var cottage = await _cottageDbContext.Cottages.FirstOrDefaultAsync(c => c.Id == id);
			return cottage;
		}

		public async Task<Cottage> CreateCottageAsync(Cottage cottage)
		{
			await CheckCottageNumberUnique(cottage.CottageNumber);

			_cottageDbContext.Cottages.Add(cottage);
			await _cottageDbContext.SaveChangesAsync();
			return cottage;
		}

		public async Task<Cottage> UpdateCottageAsync(Cottage cottage)
		{
			_cottageDbContext.Cottages.Update(cottage);
			await _cottageDbContext.SaveChangesAsync();
			return cottage;
		}

		public async Task DeleteCottageAsync(int cottageId)
		{

			var cottage = await GetCottageByIdAsync(cottageId);

			_cottageDbContext.Cottages.Remove(cottage);
			await _cottageDbContext.SaveChangesAsync();
		}

		private async Task CheckCottageNumberUnique(string cottageNumber)
		{
			if (!(await IsCottageNumberUnique(cottageNumber)))
			{
				throw new ValidationException("cottageNumber", "Коттедж с таким номером уже существует.");
			}
		}

		private async Task<bool> IsCottageNumberUnique(string cottageNumber)
		{
			var existingCottage = await _cottageDbContext.Cottages.FirstOrDefaultAsync(c => c.CottageNumber == cottageNumber);
			return existingCottage == null;
		}
	}
}
