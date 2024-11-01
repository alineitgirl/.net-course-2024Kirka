using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        Task AddAccountAsync(Guid id, Account account, CancellationToken cancellationToken);
        Task UpdateAccountAsync(Guid id, Account oldAccount, Account newAccount, CancellationToken cancellationToken);
        Task DeleteAccountAsync(Guid id, CancellationToken cancellationToken);

        Task UpdateAsync(Guid id, Client client, CancellationToken cancellationToken);
        
        Task<Client?> GetClientWithAccountsAsync(Guid id, CancellationToken cancellationToken);
    }
}