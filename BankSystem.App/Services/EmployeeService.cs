using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private IEmployeeStorage _employeeStorage;

        public EmployeeService(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }
        
        public async Task AddEmployeeAsync(Employee newEmployee)
        {
            if (newEmployee.Age < 18)
            {
                throw new AgeOutOfRangeException("Сотрудник не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newEmployee.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у сотрудника!");
            }
            
             await _employeeStorage.AddAsync(newEmployee);
        }
        
        public async Task UpdateEmployeeAsync(Guid id, Employee employee)
        => await _employeeStorage.UpdateAsync(id, employee);
        
        public async Task<List<Employee>> GetByFilterAsync(
            Expression<Func<Employee, bool>> filter = null, Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = null, 
            int pageNumber = 1, int pageSize = 1)
        {
            var employees = await _employeeStorage.GetByFilterAsync(filter, orderBy, pageNumber, pageSize);
            return employees.ToList();
        }

        public async Task<Employee?> GetByIdAsync(Guid id) => await _employeeStorage.GetByIdAsync(id);
        
        public async Task DeleteEmployee(Guid id) => await _employeeStorage.DeleteAsync(id);
    }
}