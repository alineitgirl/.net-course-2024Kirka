using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<KeyValuePair<Client, List<Account>>>
    {
        void AddAccount(KeyValuePair<Client, List<Account>> client, Account account);
        void UpdateAccount(KeyValuePair<Client, List<Account>> client, Account oldAccount, Account newAccount);
        void DeleteAccount(KeyValuePair<Client, List<Account>> client, Account accountToDelete);

    }
}