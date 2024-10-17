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

        public Employee? GetById(Guid id)
        {
            return _dbContext.Employees.AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }
        
        public IEnumerable<Employee> GetByFilter(
            Expression<Func<Employee, bool>> filter = null, Func<Employee, object> orderBy = null, 
            Func<Employee, object> groupBy = null, int pageNumber = 1, int pageSize = 1)
        {
            var query = _dbContext.Employees.AsQueryable()
                .Where(filter).OrderBy(orderBy).GroupBy(groupBy);

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .SelectMany(g => g);
        }

        public void Add(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }
        
        public void Update(Guid id, Employee employee)
        {
            _dbContext.Employees
                .Where(c => c.Id == id)
                .ExecuteUpdate(s => s
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
            _dbContext.SaveChanges();
        }
        
        public void Delete(Guid id)
        {
            _dbContext.Employees
                .Where(c => c.Id == id)
                .ExecuteDelete();
            _dbContext.SaveChanges();
        }

    }
}