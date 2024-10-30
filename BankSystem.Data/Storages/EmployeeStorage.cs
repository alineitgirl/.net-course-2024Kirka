using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BankSystem.App.Interfaces;
using BankSystem.Data.DbContext;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private readonly BankSystemDbContext _dbContext;

        public EmployeeStorage(BankSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Employees.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<List<Employee>> GetByFilterAsync(
            Expression<Func<Employee, bool>> filter = null, Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = null, 
            int pageNumber = 1, int pageSize = 1)
        {
            var query = _dbContext.Employees.AsQueryable()
                .Where(filter);
            query = orderBy != null ? orderBy(query) : query;

             return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
        }

        public async Task AddAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(Guid id, Employee employee)
        {
            await _dbContext.Employees
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.FirstName, employee.FirstName)
                    .SetProperty(s => s.LastName, employee.LastName)
                    .SetProperty(s => s.DateOfBirth, employee.DateOfBirth)
                    .SetProperty(s => s.PhoneNumber, employee.PhoneNumber)
                    .SetProperty(s => s.Passport, employee.Passport)
                    .SetProperty(s => s.Adress, employee.Adress)
                    .SetProperty(s => s.Age, employee.Age)
                    .SetProperty(s => s.Position, employee.Position)
                    .SetProperty(s => s.Salary, employee.Salary)
                    .SetProperty(s => s.Department, employee.Department)
                );
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(Guid id)
        {
            await _dbContext.Employees
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }
    }
}