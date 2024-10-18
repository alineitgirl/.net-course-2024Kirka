using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        
        public void AddEmployee(Employee newEmployee)
        {
            if (newEmployee.Age < 18)
            {
                throw new AgeOutOfRangeException("Сотрудник не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newEmployee.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у сотрудника!");
            }
            
            _employeeStorage.Add(newEmployee);

        }
        
        public void UpdateEmployee(Guid id, Employee employee)
        => _employeeStorage.Update(id, employee);
        
        public List<Employee> GetByFilter(
            Expression<Func<Employee, bool>> filter = null, Func<Employee, object> orderBy = null, 
            Func<Employee, object> groupBy =  null, int pageNumber = 1, int pageSize = 1)
        => _employeeStorage.GetByFilter(filter, orderBy, groupBy, pageNumber, pageSize).ToList();
        
        public Employee? GetById(Guid id) => _employeeStorage.GetById(id);
        
        public void DeleteEmployee(Guid id) => _employeeStorage.Delete(id);
    }
}