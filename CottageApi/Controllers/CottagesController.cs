using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Models.Cottages;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
	[Route("api/[controller]")]
	public class CottagesController : BaseApiController
	{
		private readonly IMapper _mapper;
		private readonly ICottageService _cottageService;

		public CottagesController(
			ICottageService cottageService,
			IMapper mapper)
		{
			_mapper = mapper;
			_cottageService = cottageService;
		}

		[HttpGet]
		public async Task<object> GetCottages(
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null,
			[FromQuery]bool? withoutOwners = false)
		{
			var entities = await _cottageService.GetCottagesAsync(offset, limit, withoutOwners);
			var cottages = _mapper.Map<List<CottageViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				Cottages = cottages
			};
		}

		[HttpGet("{id}")]
		public async Task<object> GetCottageById([FromRoute]int id)
		{
			var cottage = await _cottageService.GetCottageByIdAsync(id);
			var cottageView = _mapper.Map<CottageViewModel>(cottage);
			return cottageView;
		}

		[HttpPost]
		public async Task<IActionResult> CreateCottage(Cottage cottageToCreate)
		{
			var cottage = await _cottageService.CreateCottageAsync(cottageToCreate);

			return Ok(cottage);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateCottage(Cottage cottageToUpdate)
		{
			var cottage = await _cottageService.UpdateCottageAsync(cottageToUpdate);

			return Ok(cottage);
		}
	}
}
