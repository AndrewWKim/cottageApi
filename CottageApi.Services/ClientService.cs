using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Core.Domain.Dto.Users;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Exceptions;
using CottageApi.Core.Extensions;
using CottageApi.Core.Helpers;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CottageApi.Services
{
	public class ClientService : IClientService
	{
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		private readonly ICottageDbContext _cottageDbContext;

		public ClientService(
			IMapper mapper,
			ILogger<ClientService> logger,
			ICottageDbContext cottageDbContext)
		{
			_mapper = mapper;
			_logger = logger;
			_cottageDbContext = cottageDbContext;
		}

		public async Task<Tuple<IEnumerable<Client>, int>> GetClientsAsync(ClientType clientType, int? offset = null, int? limit = null)
		{

			IQueryable<Client> query = _cottageDbContext.Clients.Include(c => c.Cottage).Include(c => c.Cards).Include(c => c.User);

			switch (clientType)
			{
				case ClientType.All:
					break;
				default:
					query = query.Where(c => c.ClientType == clientType);
					break;
			}

			int total = query.Count();

			var clients = await query
				.OrderBy(c => c.CottageId).ThenBy(c => c.FirstName).ThenBy(c => c.LastName)
				.Paging(offset, limit)
				.ToListAsync();

			return new Tuple<IEnumerable<Client>, int>(clients, total);
		}

		public async Task<IEnumerable<Client>> GetCottageClientsAsync(int cottageId, int userId, bool? exceptCurrentClient = false)
		{

			IQueryable<Client> query = _cottageDbContext.Clients.Include(c => c.Cottage).Include(c => c.Cards).Where(c => c.CottageId == cottageId);

			query = exceptCurrentClient.Value ? query.Where(c => c.UserId != userId) : query;

			var clients = await query
				.OrderBy(c => c.ResidentTypeId).ThenBy(c => c.FirstName)
				.ToListAsync();

			return clients;
		}

		public async Task<Client> GetClientByIdAsync(int id)
		{

			var client = await _cottageDbContext.Clients
				.Include(c => c.Cottage)
				.Include(c => c.User)
				.Include(c => c.Cars)
				.Include(c => c.Cards)
				.Include(c => c.ResidentType)
				.FirstOrDefaultAsync(d => d.Id == id);

			if (client == null)
			{
				throw new NotFoundException();
			}

			client.PhotoUrl = client.PhotoUrl == null ? null : FilesHelper.ConvertFileToBase64(client.PhotoUrl);

			return client;
		}

		public async Task<Client> GetCurrentClientAsync(int userId)
		{

			var client = await _cottageDbContext.Clients
				.Include(c => c.Cottage)
				.Include(c => c.User)
				.Include(c => c.Cars)
				.Include(c => c.ResidentType)
				.Include(c => c.Cards)
				.FirstOrDefaultAsync(d => d.UserId == userId);

			if (client == null)
			{
				throw new NotFoundException();
			}

			client.PhotoUrl = client.PhotoUrl == null ? null : FilesHelper.ConvertFileToBase64(client.PhotoUrl);

			return client;
		}

		public async Task<Client> CreateClientAsync(Client client)
		{
			await ChangeCottageVotePayOnlySingleClient(client.CanVote, client.CanPay, client.CottageId);

			client.RegistrationCode = AuthCodeGenerator.GenerateNewCode(0, 999999);

			_cottageDbContext.Clients.Add(client);
			await _cottageDbContext.SaveChangesAsync();
			return client;
		}

		public async Task<Client> UpdateClientAsync(Client client, string photoExtension)
		{
			await ChangeCottageVotePayOnlySingleClient(client.CanVote, client.CanPay, client.CottageId, client.Id);

			var currentClient = await _cottageDbContext.Clients.Include(c => c.Cards).AsNoTracking().FirstOrDefaultAsync(c => c.Id == client.Id);

			if (client.ClientType != currentClient.ClientType)
			{
				var user = await _cottageDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == currentClient.UserId);
				if (user != null)
				{
					user.Role = ClientTypeToUserRole(client.ClientType);
					_cottageDbContext.Users.Update(user);
					await _cottageDbContext.SaveChangesAsync();
				}
			}

			client.UserId = currentClient.UserId;
			client.RegistrationCode = currentClient.RegistrationCode;
			client.Cards = currentClient.Cards;
			client.PayCount = currentClient.PayCount;

			if (client.PhotoUrl == null && currentClient.PhotoUrl != null)
			{
				FilesHelper.DeleteFile(currentClient.PhotoUrl);
			}
			else if (client.PhotoUrl != null)
			{
				client.PhotoUrl = client.PhotoUrl.StartsWith("data") ? currentClient.PhotoUrl : FilesHelper.SavePhoto(client.PhotoUrl, photoExtension, currentClient.PhotoUrl);
			}

			var currentClientCars = await _cottageDbContext.Cars.AsNoTracking().Where(c => c.ClientId == client.Id).ToListAsync();

			foreach (var car in currentClientCars)
			{
				if (client.Cars.FirstOrDefault(с => с.Id == car.Id) == null)
				{
					_cottageDbContext.Cars.Remove(car);
					await _cottageDbContext.SaveChangesAsync();
				}
			}

			_cottageDbContext.Clients.Update(client);
			await _cottageDbContext.SaveChangesAsync();

			return client;
		}

        public async Task UpdateClientBiometricsSignature(int userId, string signature)
        {
            var user = await _cottageDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.BiometricsSignature = signature;
            _cottageDbContext.Users.Update(user);
            await _cottageDbContext.SaveChangesAsync();
		}

		public async Task CreateClientsUser(CreateUserDto userDto)
		{
			var client = await _cottageDbContext.Clients
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.RegistrationCode == userDto.RegistrationCode);

			if (client == null || client.User != null)
			{
				throw new ValidationException("registrationCode", "Код регистрации не действителен.");
			}

			if (client.ClientType == ClientType.Resident)
			{
				throw new ValidationException("registrationCode", "Доступ вам закрыт.");
			}

			var existingUser = await _cottageDbContext
				.Users.FirstOrDefaultAsync(u => u.Name == userDto.Username);

			if (existingUser != null)
			{
				throw new ValidationException("userName", "Логин уже занят.");
			}

			var user = _mapper.Map<User>(userDto);

			user.Role = ClientTypeToUserRole(client.ClientType);

			user.Password = EncryptionHelper.HashWithMD5(user.Password);

			client.User = user;

			_cottageDbContext.Clients.Update(client);
			await _cottageDbContext.SaveChangesAsync();
		}

		public async Task CreateClientCardAsync(int clientId, CreateCardDto createCardDto)
		{
			/*var existingCard = await _cottageDbContext.Cards.FirstOrDefaultAsync(c => c.ClientId == clientId && c.CardNumber == createCardDto.CardNumber);
			if (existingCard != null)
			{
				throw new ValidationException("сardNumber", "Карта уже добавлена для этого клиента.");
			}

			var cardMask = createCardDto.CardNumber.Substring(0, 2) + "****" + createCardDto.CardNumber.Substring(12, 4);

			var newCard = new Card()
			{
				CardMask = cardMask,
				CardNumber = createCardDto.CardNumber,
				CardExpMonth = createCardDto.CardExpMonth,
				CardExpYear = createCardDto.CardExpYear,
				ClientId = clientId
			};

			_cottageDbContext.Cards.Add(newCard);
			await _cottageDbContext.SaveChangesAsync();*/
		}

		public async Task<Tuple<IEnumerable<Car>, int>> GetClientsCarsAsync(string carLicensePlate, int? offset = null, int? limit = null)
		{

			IQueryable<Car> query = _cottageDbContext.Cars.Include(c => c.Client);

			if (!string.IsNullOrEmpty(carLicensePlate))
			{
				query = query.Where(c => c.CarLicensePlate.Contains(carLicensePlate));
			}


			int total = query.Count();

			var cars = await query
				.OrderBy(c => c.Client.CottageId)
				.Paging(offset, limit)
				.ToListAsync();

			return new Tuple<IEnumerable<Car>, int>(cars, total);
		}

		private UserRole ClientTypeToUserRole(ClientType clientType)
		{
			var role = UserRole.Resident;

			switch (clientType)
			{
				case ClientType.Owner:
					role = UserRole.Owner;
					break;
				case ClientType.Resident:
					break;
				case ClientType.MainResident:
					role = UserRole.MainResident;
					break;
				default:
					break;
			}

			return role;
		}

		private async Task ChangeCottageVotePayOnlySingleClient(bool canVote, bool canPay, int cottageId, int? clientId = null)
		{
			var clients = await _cottageDbContext.Clients.AsNoTracking().Where(c => c.CottageId == cottageId).ToListAsync();
			clients.Remove(clients.FirstOrDefault(c => c.Id == clientId));
			var cottageOwner = clients.FirstOrDefault(c => c.ClientType == ClientType.Owner);

			if (clients.Count() == 0)
			{
				return;
			}

			if (canVote)
			{
				foreach (var client in clients)
				{
					if (client.CanVote)
					{
						client.CanVote = false;
						_cottageDbContext.Clients.Update(client);
						await _cottageDbContext.SaveChangesAsync();
					}
				}
			}

			if (canPay)
			{
				foreach (var client in clients)
				{
					if (client.CanPay)
					{
						client.CanPay = false;
						_cottageDbContext.Clients.Update(client);
						await _cottageDbContext.SaveChangesAsync();
					}
				}
			}

			if (!canVote && !clients.Any(c => c.CanVote) && cottageOwner != null)
			{
				cottageOwner.CanVote = true;
				_cottageDbContext.Clients.Update(cottageOwner);
				await _cottageDbContext.SaveChangesAsync();
			}

			if (!canPay && !clients.Any(c => c.CanPay) && cottageOwner != null)
			{
				cottageOwner.CanPay = true;
				_cottageDbContext.Clients.Update(cottageOwner);
				await _cottageDbContext.SaveChangesAsync();
			}
		}
	}
}