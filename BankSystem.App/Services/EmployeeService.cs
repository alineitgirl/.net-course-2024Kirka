using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public void UpdateEmployee (Employee oldEmployee, Employee employeeToUpdate)
        {
            _employeeStorage.Update(oldEmployee, employeeToUpdate);
        }
        

        public List<Employee>? GetEmployeesByFilter(Func<Employee, bool> filter = null)
        {
            return _employeeStorage.Get(filter).ToList();
        }
    }
}