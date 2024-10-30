using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        Task AddAccountAsync(Guid id, Account account);
        Task UpdateAccountAsync(Guid id, Account oldAccount, Account newAccount);
        Task DeleteAccountAsync(Guid id);

        Task UpdateAsync(Guid id, Client client);
        
        Task<Client?> GetClientWithAccountsAsync(Guid id);
    }
}