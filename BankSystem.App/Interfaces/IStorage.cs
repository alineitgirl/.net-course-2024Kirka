using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T>
    {
        IEnumerable<T> Get(Func<T, bool> filter);
        void Add(T item);
        void Update(T oldItem, T newItem);
        void Delete(T item);
    }
}