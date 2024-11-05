using System;
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
        
        public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            return await _dbContext.Clients.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<List<Client>> GetByFilterAsync(
            Expression<Func<Client, bool>> filter = null, 
            Func<IQueryable<Client>, IOrderedQueryable<Client>> orderBy = null,
            int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            var query = _dbContext.Clients.Where(filter);
            query = orderBy != null ? orderBy(query) : query;
            
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            return await query.ToListAsync(cancellationToken);
        }


        public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _dbContext.Clients.AddAsync(client, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await AddDefaultUsdAccountToClient(client.Id);
        }
        
        public async Task UpdateAsync(Guid id, Client client, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            if (_dbContext.Clients.Any(c => c.Id == id))
            {
               await _dbContext.Clients
                    .Where(c => c.Id == id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(s => s.FirstName, client.FirstName)
                        .SetProperty(s => s.LastName, client.LastName)
                        .SetProperty(s => s.DateOfBirth, client.DateOfBirth)
                        .SetProperty(s => s.PhoneNumber, client.PhoneNumber)
                        .SetProperty(s => s.Passport, client.Passport)
                        .SetProperty(s => s.Adress, client.Adress)
                        .SetProperty(s => s.Age, client.Age), cancellationToken);
            }
                
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _dbContext.Clients
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task AddAccountAsync(Guid id, Account account, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            account.ClientId = id;
            await _dbContext.Accounts
               .AddAsync(new Account
               {
                   Amount = account.Amount,
                   CurrencyName = account.CurrencyName,
                   ClientId = id
               }, cancellationToken);
        }

        private async Task AddDefaultUsdAccountToClient(Guid id)
        {
            await _dbContext.Accounts.AddAsync(new Account
            {
                ClientId = id,
                CurrencyName = "USD"
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Guid id, Account oldAccount, Account newAccount, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _dbContext.Accounts
                .Where(a => a.ClientId == id && a.Id == oldAccount.Id)
                .ExecuteUpdateAsync(a => a
                    .SetProperty(x => x.Amount, newAccount.Amount)
                    .SetProperty(x => x.CurrencyName, newAccount.CurrencyName), cancellationToken);
        }

        public async Task DeleteAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _dbContext.Accounts
               .Where(a => a.Id == id)
               .ExecuteDeleteAsync(cancellationToken);
        }
        public async Task<Client?> GetClientWithAccountsAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            return await _dbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);
        }
    }
}