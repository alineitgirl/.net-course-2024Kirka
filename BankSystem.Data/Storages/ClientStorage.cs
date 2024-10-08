using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private Dictionary<Client, List<Account>> _dictionaryOfClients;

        public ClientStorage(Dictionary<Client, List<Account>> clients)
        {
            _dictionaryOfClients = clients;
        }
        
        public IEnumerable<KeyValuePair<Client, List<Account>>> Get(Func<KeyValuePair<Client, List<Account>>, bool> filter = null)
        {
            if (filter is null)
            {
                return _dictionaryOfClients;
            }
            var selectedClients = _dictionaryOfClients.Where(filter);
            return selectedClients;
        }


        public void Add(KeyValuePair<Client, List<Account>> client)
        {
            if (!(_dictionaryOfClients.Contains(client)))
            {
                _dictionaryOfClients.Add(client.Key, client.Value);
            }
            AddDefaultUsdAccountToClient(client.Value);
        }
        
        public void Update(KeyValuePair<Client, List<Account>> oldClient, KeyValuePair<Client, List<Account>> newClient)
        {
            if (!(_dictionaryOfClients.Contains(oldClient)))
            {
                Add(oldClient);
                return;
            }

            var searchedClient = _dictionaryOfClients.FirstOrDefault(cl => 
                cl.Key.Equals(oldClient.Key));
                searchedClient.Key.FirstName= newClient.Key.FirstName;
                searchedClient.Key.LastName = newClient.Key.LastName;
                searchedClient.Key.DateOfBirth = newClient.Key.DateOfBirth;
                searchedClient.Key.Adress = newClient.Key.Adress;
                searchedClient.Key.Passport = newClient.Key.Passport;
                searchedClient.Key.PhoneNumber = newClient.Key.PhoneNumber;
                searchedClient.Key.Id= newClient.Key.Id;
                searchedClient.Key.AccountBalance = newClient.Key.AccountBalance;
                searchedClient.Key.Age = newClient.Key.Age;
        }
        
        public void Delete(KeyValuePair<Client, List<Account>> client)
        {
            if ((_dictionaryOfClients.Contains(client)))
            {
                _dictionaryOfClients.Remove(client.Key);
            }
        }


        public void AddAccount(KeyValuePair<Client, List<Account>> client, Account account)
        {
            if (_dictionaryOfClients.TryGetValue(client.Key, out var accounts))
            {
                accounts.Add(account);
            }
        }

        private void AddDefaultUsdAccountToClient(List<Account> accounts)
        {
            accounts.Add(new Account {Amount = 0, Currency = "USD"});
        }

        public void UpdateAccount(KeyValuePair<Client, List<Account>> client, Account oldAccount,
            Account updatedAccount)
        {
            if (!(client.Value.Contains(oldAccount)))
            {
                AddAccount(client, updatedAccount);
                return;
            }
            var searchedAccount = client.Value.FirstOrDefault(acc => acc.Equals(oldAccount));
            searchedAccount.Amount = updatedAccount.Amount;
            searchedAccount.Currency = updatedAccount.Currency;
        }

        public void DeleteAccount(KeyValuePair<Client, List<Account>> client, Account accountToDelete)
        {
            client.Value.RemoveAll(cl => cl.Equals(accountToDelete));
        }
    }
}