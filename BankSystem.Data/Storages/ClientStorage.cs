using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class ClientStorage
    {
        private Dictionary<Client, List<Account>> _dictionaryOfClients;

        public ClientStorage(Dictionary<Client, List<Account>> clients)
        {
            _dictionaryOfClients = clients;
        }

        public bool AddClientToStorage(KeyValuePair<Client, List<Account>> client)
        {
            if (!(_dictionaryOfClients.Contains(client)))
            {
                _dictionaryOfClients.Add(client.Key, client.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateClientFromStorage(KeyValuePair<Client, List<Account>> client)
        {
            if (_dictionaryOfClients.Contains(client))
            {
                _dictionaryOfClients[client.Key] = client.Value;
            }
            else
            {
                AddClientToStorage(client);
            }
        }

        public bool DeleteClientFromStorage(KeyValuePair<Client, List<Account>> client)
        {
            if ((_dictionaryOfClients.Contains(client)))
            {
                _dictionaryOfClients.Remove(client.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SearchClientInStorage(Client client)
        {
            return _dictionaryOfClients.TryGetValue(client, out List<Account> accounts);
        }

        public KeyValuePair<Client, List<Account>> FindTheYoungestClient()
        {
            var minAge = _dictionaryOfClients.Min(cl => cl.Key.Age);
            var clientsWithMinAge = _dictionaryOfClients.Where(cl => cl.Key.Age == minAge).ToList();
            return clientsWithMinAge.FirstOrDefault(cl => cl.Key.Age == minAge);
        }

        public KeyValuePair<Client, List<Account>> FindTheOldestClient()
        {
            var maxAge = _dictionaryOfClients.Max(cl => cl.Key.Age);
            var clientsWithMaxAge = _dictionaryOfClients.Where(cl => cl.Key.Age == maxAge).ToList();
            return clientsWithMaxAge.FirstOrDefault(cl => cl.Key.Age == maxAge);
        }

        public double CalculateAverageAgeOfClients()
        {
           return  _dictionaryOfClients.Average(cl => cl.Key.Age);
        }
    }
}