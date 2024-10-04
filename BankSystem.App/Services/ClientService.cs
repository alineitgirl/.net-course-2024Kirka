using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using BankSystem.Data.Storages;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private ClientStorage _clientStorage;

        public ClientService(ClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public void AddClientToService(KeyValuePair<Client, List<Account>> newClient)
        {
            if (newClient.Key.Age < 18)
            {
                throw new AgeOutOfRangeException("Клиент не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newClient.Key.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у клиента!");
            }

            if (_clientStorage.SearchClientInStorage(newClient.Key)) return;
            _addDefaultUsdAccountToClient(newClient.Value);
            _clientStorage.AddClientToStorage(newClient);

        }

        private void _addDefaultUsdAccountToClient(List<Account> accounts)
        {
            accounts.Add(new Account {Amount = 0, Currency = "USD"});
        }

        public void AddNewAccountToClient(KeyValuePair<Client, List<Account>> client, Account account)
        {
            if (_clientStorage.SearchClientInStorage(client.Key))
            {
                var searchedClient = _clientStorage.DictionaryOfClients.FirstOrDefault(cl 
                    => cl.Key.Equals(client.Key));
                searchedClient.Value.Add(account);
            }
        }

        public void UpdateAddedAccountOfClient(KeyValuePair<Client, List<Account>> client, Account oldAccount,
            Account updatedAccount)
        {
            if (!(client.Value.Contains(oldAccount)))
            {
                AddNewAccountToClient(client, updatedAccount);
            }
            var searchedAccount = client.Value.FirstOrDefault(acc => acc.Equals(oldAccount));
                searchedAccount.Amount = updatedAccount.Amount;
                searchedAccount.Currency = updatedAccount.Currency;
        }

        public Dictionary<Client, List<Account>> GetClientsByFilter(Func<Client, bool> filter = null)
        {
            if (filter is null)
            {
                return _clientStorage.DictionaryOfClients.ToDictionary(cl => cl.Key,
                    cl => cl.Value);
            }
            var selectedClients = _clientStorage.DictionaryOfClients.Where(cl => filter(cl.Key))
                .ToDictionary(cl => cl.Key, cl => cl.Value);
            return selectedClients;
        }
        
    }
}