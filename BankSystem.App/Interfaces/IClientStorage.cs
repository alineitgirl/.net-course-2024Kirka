using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        void AddAccount(Guid id, Account account);
        void UpdateAccount(Guid id, Account oldAccount, Account newAccount);
        void DeleteAccount(Guid id);

        void Update(Guid id, Client client);

    }
}