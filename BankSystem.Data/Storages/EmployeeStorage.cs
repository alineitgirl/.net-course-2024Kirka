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
        
        public void Update(Guid id, string firstName, string lastName, DateTime birthDate,
            string phoneNumber, string passport, string address, int age, string position, 
            double salary, string department)
        {
            _dbContext.Employees
                .Where(c => c.Id == id)
                .ExecuteUpdate(s => s
                    .SetProperty(s => s.FirstName, firstName)
                    .SetProperty(s => s.LastName, lastName)
                    .SetProperty(s => s.DateOfBirth, birthDate)
                    .SetProperty(s => s.PhoneNumber, phoneNumber)
                    .SetProperty(s => s.Passport, passport)
                    .SetProperty(s => s.Adress, address)
                    .SetProperty(s => s.Age, age)
                    .SetProperty(s => s.Position, position)
                    .SetProperty(s => s.Salary, salary)
                    .SetProperty(s => s.Department, department)
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