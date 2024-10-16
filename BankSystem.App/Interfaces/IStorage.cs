using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T>
    {
        IEnumerable<T> GetByFilter(Expression<Func<T, bool>> filter, Func<T, object> orderBy, 
            Func<T, object> groupBy, int pageNumber, int pageSize);
        void Add(T item);
        void Delete(Guid id);
        T GetById(Guid id);
    }
}