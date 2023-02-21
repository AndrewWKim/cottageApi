using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Dto.Users;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Services;
using CottageApi.Models.Cars;
using CottageApi.Models.Clients;
using CottageApi.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CottageApi.Controllers
{
	[Route("api/[controller]")]
	public class ClientsController : BaseApiController
	{
		private readonly IClientService _clientService;
		private readonly IMapper _mapper;

		public ClientsController(
			IClientService clientService,
			IMapper mapper)
		{
			_clientService = clientService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<object> GetClients(
			[FromQuery]ClientType clientType,
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null)
		{
			var entities = await _clientService.GetClientsAsync(clientType, offset, limit);
			var clients = _mapper.Map<List<ClientViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				Clients = clients
			};
		}

		[HttpGet("cottage/{cottageId}")]
		public async Task<object> GetCottageClients(
			[FromRoute]int cottageId,
			[FromQuery]bool? exceptCurrentClient = false)
		{
			var entities = await _clientService.GetCottageClientsAsync(cottageId, ClaimsUserId, exceptCurrentClient);
			var clients = _mapper.Map<List<ClientViewModel>>(entities);
			return clients;
		}

		[HttpGet("{id}")]
		public async Task<object> GetClientById([FromRoute]int id)
		{
			var client = await _clientService.GetClientByIdAsync(id);
			var clientEditView = _mapper.Map<ClientViewModel>(client);
			return clientEditView;
		}

		[HttpGet("current")]
		public async Task<object> GetCurrentClient()
		{
			var client = await _clientService.GetCurrentClientAsync(ClaimsUserId);
			var clientEditView = _mapper.Map<ClientViewModel>(client);
			return clientEditView;
		}

		[HttpPost]
		public async Task<IActionResult> CreateClient(CreateClientModel createClientModel)
		{
			var clientToCreate = _mapper.Map<Client>(createClientModel);
			var client = await _clientService.CreateClientAsync(clientToCreate);

			return Ok(client);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateClient(EditClientModel editClientModel)
		{
			var clientToCreate = _mapper.Map<Client>(editClientModel);
			var client = await _clientService.UpdateClientAsync(clientToCreate, editClientModel.PhotoExtension);

			return Ok(client);
		}

		[HttpPut("biometrics")]
		public async Task<IActionResult> UpdateClientBiometrics(UpdateBiometricModel biometric)
        {
            await _clientService.UpdateClientBiometricsSignature(ClaimsUserId, biometric.Signature);

            return Ok();
        }

		[AllowAnonymous]
		[HttpPost("user")]
		public async Task<IActionResult> CreateUserForClient(CreateUserModel createUserModel)
		{
			var userToCreate = _mapper.Map<CreateUserDto>(createUserModel);
			await _clientService.CreateClientsUser(userToCreate);

			return Ok();
		}

		[HttpPost("{id}/card")]
		public async Task<IActionResult> CreateClientCard([FromRoute]int id, CreateCardDto createCardDto)
		{
			await _clientService.CreateClientCardAsync(id, createCardDto);

			return Ok();
		}

		[HttpGet("cars")]
		public async Task<object> GetClientsCars(
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null,
			[FromQuery]string carLicensePlate = null)
		{
			var entities = await _clientService.GetClientsCarsAsync(carLicensePlate, offset, limit);
			var cars = _mapper.Map<List<CarViewModel>>(entities.Item1);
			return new
			{
				Total = entities.Item2,
				Cars = cars
			};
		}
	}
}