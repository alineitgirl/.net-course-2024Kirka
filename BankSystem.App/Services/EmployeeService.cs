using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private EmployeeStorage _employeeStorage;

        public EmployeeService(EmployeeStorage employeeStorage)
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
            
            _employeeStorage.AddEmployeeToStorage(newEmployee);

        }
        
        public void UpdateEmployee (Employee oldEmployee, Employee newEmployee)
        {
            _employeeStorage.UpdateEmployeeFromStorage(oldEmployee, newEmployee);
        }
        

        public List<Employee>? GetEmployeesByFilter(Func<Employee, bool> filter = null)
        {
            return _employeeStorage.GetEmployeesByFilter(filter);
        }
    }
}