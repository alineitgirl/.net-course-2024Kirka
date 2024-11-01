using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private IClientStorage _clientStorage;

        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public async Task AddClientAsync(Client newClient, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            if (newClient.Age < 18)
            {
                throw new AgeOutOfRangeException("Клиент не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newClient.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у клиента!");
            }

            if (_clientStorage.GetByIdAsync(newClient.Id, cancellationToken) != null) return;
            await  _clientStorage.AddAsync(newClient, cancellationToken);
        }

        public async Task UpdateClientAsync(Guid id, Client client, CancellationToken cancellationToken)
        {
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

        public async Task<Client> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            return await _clientStorage.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<Client>> GetByFilterAsync(
            Expression<Func<Client, bool>> filter, Func<IQueryable<Client>, IOrderedQueryable<Client>> orderBy, 
            int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            var clients = await _clientStorage.GetByFilterAsync(filter, orderBy, pageNumber, pageSize, cancellationToken);
            return clients;
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