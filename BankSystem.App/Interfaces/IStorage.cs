using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T>
    {
        Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize);
        Task AddAsync(T item);
        Task DeleteAsync(Guid id);
        Task<T> GetByIdAsync(Guid id);
    }
}