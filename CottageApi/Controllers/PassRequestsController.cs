using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Services;
using CottageApi.Models.PassRequests;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
	[Route("api/[controller]")]
	public class PassRequestsController : BaseApiController
	{
		private readonly IPassRequestService _passRequestService;
		private readonly IMapper _mapper;

		public PassRequestsController(
			IPassRequestService passRequestService,
			IMapper mapper)
		{
			_passRequestService = passRequestService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<object> GetPassRequests(
			[FromQuery]DateTime? date = null,
			[FromQuery]int? cottageId = null,
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null)
		{
			var entities = await _passRequestService.GetPassRequests(date, cottageId, offset, limit);
			var passRequests = _mapper.Map<List<PassRequestViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				PassRequests = passRequests
			};
		}

		[HttpGet("cottage/{cottageId}")]
		public async Task<object> GetPassRequestsForMobile([FromRoute]int cottageId)
		{
			var entities = await _passRequestService.GetPassRequestsForMobile(cottageId);
			var passRequests = _mapper.Map<List<PassRequestMobileViewModel>>(entities);
			return passRequests;
		}

		[HttpPost]
		public async Task<IActionResult> CreatePassRequest(PassRequest passRequestToCreate)
		{
			var passRequest = await _passRequestService.CreatePassRequest(passRequestToCreate);

			return Ok(passRequest);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePassRequest([FromRoute]int id)
		{
			await _passRequestService.DeletePassRequest(id);

			return Ok();
		}
	}
}
