using System;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeStorage : IStorage<Employee>
    {
        public Task UpdateAsync(Guid id, Employee employee, CancellationToken cancellationToken);
    }
}