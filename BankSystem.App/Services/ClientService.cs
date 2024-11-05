using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using AutoMapper;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private IClientStorage _clientStorage;
        private readonly IMapper _mapper;

        public ClientService(IClientStorage clientStorage, IMapper mapper)
        {
            _clientStorage = clientStorage;
            _mapper = mapper;
        }

        public async Task AddClientAsync(ClientDto newClient, CancellationToken cancellationToken)
        {
            var client = _mapper.Map<Client>(newClient);
            client.Id = Guid.NewGuid();
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            if (_clientStorage.GetByIdAsync(client.Id, cancellationToken) != null) return;
            await  _clientStorage.AddAsync(client, cancellationToken);
        }

        public async Task UpdateClientAsync(Guid id, ClientDto clientDto, CancellationToken cancellationToken)
        {
            var client = _mapper.Map<Client>(clientDto);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _clientStorage.UpdateAsync(id, client, cancellationToken);
        }

        public async Task DeleteClientAsync(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _clientStorage.DeleteAsync(id, cancellationToken);
        }

        public async Task<ClientDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            var client = await _clientStorage.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<List<ClientDto>> GetByFilterAsync(
            Expression<Func<Client, bool>> filter, Func<IQueryable<Client>, IOrderedQueryable<Client>> orderBy, 
            int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            var clients = await _clientStorage.GetByFilterAsync(filter, orderBy, pageNumber, pageSize, cancellationToken);
            return _mapper.Map<List<ClientDto>>(clients);
        }


        public async Task AddNewAccountToClientAsync(Guid id, Account newAccount, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            await _clientStorage.AddAccountAsync(id, newAccount, token);
        }

        public async Task UpdateAddedAccountOfClientAsync(Guid id, Account oldAccount, Account newAccount,
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            await _clientStorage.UpdateAccountAsync(id, oldAccount, newAccount, token);
        }

        public async Task RemoveAccountFromClientAsync(Guid id, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            await _clientStorage.DeleteAccountAsync(id, token);
        }

        public async Task<bool> WriteOffMoney(Guid clientId, Account account, double amount, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return false;
            }
            var client = await _clientStorage.GetClientWithAccountsAsync(clientId, token);
            if (client is null) return false;

            var searchedAccount = client.Accounts.FirstOrDefault(a => a.Id == account.Id);
            if (searchedAccount != null && searchedAccount.Amount >= amount)
            {
                searchedAccount.Amount -= amount;
                await _clientStorage.UpdateAccountAsync(clientId, account, searchedAccount, token);
                return true;
            }

            return false;
        }

        public async Task<Client?> GetClientWithAccountAsync(Guid clientId, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return null;
            }
            return await _clientStorage.GetClientWithAccountsAsync(clientId, token);
        }
    }
}