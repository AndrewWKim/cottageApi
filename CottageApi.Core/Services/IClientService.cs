using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Dto.Users;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Services
{
    public interface IClientService
    {
        Task<Tuple<IEnumerable<Client>, int>> GetClientsAsync(ClientType clientType, int? offset, int? limit);

        Task<IEnumerable<Client>> GetCottageClientsAsync(int cottageId, int userId, bool? exceptCurrentClient);

        Task<Client> GetClientByIdAsync(int id);

        Task<Client> GetCurrentClientAsync(int userId);

        Task<Client> CreateClientAsync(Client client);

        Task<Client> UpdateClientAsync(Client client, string photoExtension);

        Task CreateClientsUser(CreateUserDto userDto);

        Task<Tuple<IEnumerable<Car>, int>> GetClientsCarsAsync(string carLicensePlate, int? offset = null, int? limit = null);

        Task CreateClientCardAsync(int clientId, CreateCardDto createCardDto);

        Task UpdateClientBiometricsSignature(int userId, string signature);
    }
}