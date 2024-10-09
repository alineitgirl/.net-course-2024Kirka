using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            if (_clientStorage.Get(client => client.Equals(newClient)).Any()) return;
            _clientStorage.Add(newClient);

        }
        
        public void AddNewAccountToClient(KeyValuePair<Client, List<Account>> client, Account account)
        {
            _clientStorage.AddAccount(client, account);
        }

        public void UpdateAddedAccountOfClient(KeyValuePair<Client, List<Account>> client, Account oldAccount,
            Account updatedAccount)
        {
            _clientStorage.UpdateAccount(client, oldAccount, updatedAccount);
        }

        public Dictionary<Client, List<Account>> GetClientsByFilter(Func<KeyValuePair<Client, List<Account>>, bool> filter = null)
        {
            return  _clientStorage.Get(filter).ToDictionary(cl => cl.Key, 
                cl => cl.Value);
        }
        
    }
}