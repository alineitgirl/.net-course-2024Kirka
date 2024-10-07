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

        public void AddClient(KeyValuePair<Client, List<Account>> newClient)
        {
            if (newClient.Key.Age < 18)
            {
                throw new AgeOutOfRangeException("Клиент не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newClient.Key.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у клиента!");
            }

            if (_clientStorage.GetClientsByFilter(client => client.Equals(newClient.Key)).Any()) return;
            _clientStorage.AddDefaultUsdAccountToClient(newClient.Value);
            _clientStorage.AddClientToStorage(newClient);

        }
        
        public void AddNewAccountToClient(KeyValuePair<Client, List<Account>> client, Account account)
        {
            _clientStorage.AddAccount(client, account);
        }

        public void UpdateAddedAccountOfClient(KeyValuePair<Client, List<Account>> client, Account oldAccount,
            Account updatedAccount)
        {
            _clientStorage.UpdateAccountOfClient(client, oldAccount, updatedAccount);
        }

        public Dictionary<Client, List<Account>> GetClientsByFilter(Func<Client, bool> filter = null)
        {
            return  _clientStorage.GetClientsByFilter(filter);
        }
        
    }
}