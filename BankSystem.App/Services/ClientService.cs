using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public void AddClient(Client newClient)
        {
            if (newClient.Age < 18)
            {
                throw new AgeOutOfRangeException("Клиент не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newClient.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у клиента!");
            }

            if (_clientStorage.GetById(newClient.Id) != null) return;
            _clientStorage.Add(newClient);
        }

        public void UpdateClient(Guid id, Client client)
        => _clientStorage.Update(id, client);
        
        public void DeleteClient(Guid id) => _clientStorage.Delete(id);
        public Client GetById(Guid id) => _clientStorage.GetById(id);

        public List<Client> GetByFilter(
            Expression<Func<Client, bool>> filter, Func<Client, object> orderBy, 
            Func<Client, object> groupBy, int pageNumber, int pageSize)
        => _clientStorage.GetByFilter(filter, orderBy, groupBy, pageNumber, pageSize).ToList();
        
        
        public void AddNewAccountToClient(Guid id, Account newAccount)
        => _clientStorage.AddAccount(id, newAccount);

        public void UpdateAddedAccountOfClient(Guid id, Account oldAccount, Account newAccount)
        => _clientStorage.UpdateAccount(id, oldAccount, newAccount);
        
        public void RemoveAccountFromClient(Guid id) => _clientStorage.DeleteAccount(id);
        
    }
}