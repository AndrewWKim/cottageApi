using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Dto;
using CottageApi.Core.Domain.Dto.Ideas;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Services;
using CottageApi.Models.Ideas;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
	[Route("api/[controller]")]
	public class IdeasController : BaseApiController
	{
		private readonly IIdeaService _ideaService;
		private readonly IMapper _mapper;

		public IdeasController(
			IIdeaService ideaService,
			IMapper mapper)
		{
			_ideaService = ideaService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<object> GetIdeas(
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null,
			[FromQuery]List<IdeaStatus> statuses = null)
		{
			var entities = await _ideaService.GetIdeasAsync(statuses, offset, limit);
			var ideas = _mapper.Map<List<IdeaViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				Ideas = ideas
			};
		}

		[HttpGet("{userId}")]
		public async Task<object> GetClientIdeas(
			[FromRoute]int userId,
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null)
		{
			var entities = await _ideaService.GetClientIdeasAsync(userId, offset, limit);
			var ideas = _mapper.Map<List<ClientIdeaViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				Ideas = ideas
			};
		}

		[HttpGet("idea/{id}")]
		public async Task<object> GetIdeaById(
			[FromRoute]int id)
		{
			var idea = await _ideaService.GetIdeaByIdAsync(id);
			return idea;
		}

		[HttpGet("creator/{userId}")]
		public async Task<object> GetIdeasByCreator(
			[FromRoute] int userId,
			[FromQuery] int? offset = null,
			[FromQuery] int? limit = null)
		{
			var entities = await _ideaService.GetIdeasByCreatorAsync(userId, offset, limit);
			var ideas = _mapper.Map<List<CreatorIdeaViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				Ideas = ideas
			};
		}

		[HttpPost]
		public async Task<IActionResult> CreateIdea(CreateIdeaModel createIdeaModel)
		{
			var ideaToCreate = _mapper.Map<Idea>(createIdeaModel);
			var idea = await _ideaService.CreateIdeaAsync(ideaToCreate);

			return Ok(idea);
		}

		[HttpPut]
		public async Task<IActionResult> VoteIdea(IdeaVoteDto ideaVoteDto)
		{
			await _ideaService.VoteIdeaAsync(ideaVoteDto);

			return Ok();
		}

		[HttpPut("{id}/read")]
		public async Task<IActionResult> ReadIdea([FromRoute] int id)
		{
			var ideaRead = new IdeaRead
			{
				IdeaId = id,
				UserId = ClaimsUserId
			};

			await _ideaService.ReadIdeaAsync(ideaRead);

			return Ok();
		}

		[HttpPut("edit")]
		public async Task<IActionResult> EditIdea(EditIdeaDto ideaToEdit)
		{
			await _ideaService.EditIdeaAsync(ideaToEdit);

			return Ok();
		}
	}
}