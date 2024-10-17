﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using BankSystem.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly  BankSystemDbContext _dbContext;

        public ClientStorage(BankSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Client? GetById(Guid id)
        {
            return _dbContext.Clients.AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }
        public IEnumerable<Client> GetByFilter(
            Expression<Func<Client, bool>> filter = null, Func<Client, object> orderBy =  null, 
            Func<Client, object> groupBy = null, int pageNumber = 1, int pageSize = 1)
        {
            var query = _dbContext.Clients.AsQueryable()
                .Where(filter).OrderBy(orderBy).GroupBy(groupBy);

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .SelectMany(g => g);
        }

        public void Add(Client client)
        {
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();
            AddDefaultUsdAccountToClient(client.Id);
        }
        
        public void Update(Guid id, Client client)
        {
            if (_dbContext.Clients.Any(c => c.Id == id))
            {
                _dbContext.Clients
                    .Where(c => c.Id == id)
                    .ExecuteUpdate(s => s
                        .SetProperty(s => s.FirstName, client.FirstName)
                        .SetProperty(s => s.LastName, client.LastName)
                        .SetProperty(s => s.DateOfBirth, client.DateOfBirth)
                        .SetProperty(s => s.PhoneNumber, client.PhoneNumber)
                        .SetProperty(s => s.Passport, client.Passport)
                        .SetProperty(s => s.Adress, client.Adress)
                        .SetProperty(s => s.Age, client.Age));
            }
           
                
        }
        
        public void Delete(Guid id)
        {
            _dbContext.Clients
                .Where(c => c.Id == id)
                .ExecuteDelete();
            _dbContext.SaveChanges();
        }


        public void AddAccount(Guid id, Account account)
        {
            account.ClientId = id;
           _dbContext.Accounts
               .Add(new Account
               {
                   Amount = account.Amount,
                   CurrencyName = account.CurrencyName,
                   ClientId = id
               });
        }

        private void AddDefaultUsdAccountToClient(Guid id)
        {
            _dbContext.Accounts.Add(new Account
            {
                ClientId = id,
                CurrencyName = "USD"
            });
            _dbContext.SaveChanges();
        }

        public void UpdateAccount(Guid id, Account oldAccount, Account newAccount)
        {
            _dbContext.Accounts
                .Where(a => a.ClientId == id && a.Id == oldAccount.Id)
                .ExecuteUpdate(a => a
                    .SetProperty(x => x.Amount, newAccount.Amount)
                    .SetProperty(x => x.CurrencyName, newAccount.CurrencyName));
        }

        public void DeleteAccount(Guid id)
        {
           _dbContext.Accounts
               .Where(a => a.Id == id)
               .ExecuteDelete();
        }
    }
}