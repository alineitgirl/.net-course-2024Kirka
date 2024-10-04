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
        
        public void AddEmployeeToService(Employee newEmployee)
        {
            if (newEmployee.Age < 18)
            {
                throw new AgeOutOfRangeException("Сотрудник не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(newEmployee.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у сотрудника!");
            }

            if (_employeeStorage.AddEmployeeToStorage(newEmployee)) return;
            _employeeStorage.AddEmployeeToStorage(newEmployee);

        }
        
        public void UpdateEmployeeFromService (Employee oldEmployee, Employee newEmployee)
        {
            if (!(_employeeStorage.Employees.Contains(oldEmployee)))
            {
                _employeeStorage.AddEmployeeToStorage(oldEmployee);
            }
            var searchedEmployee = _employeeStorage.Employees.FirstOrDefault(empl => empl.Equals(oldEmployee));
                searchedEmployee.FirstName= newEmployee.FirstName;
                searchedEmployee.LastName = newEmployee.LastName;
                searchedEmployee.DateOfBirth = newEmployee.DateOfBirth;
                searchedEmployee.Adress = newEmployee.Adress;
                searchedEmployee.Passport = newEmployee.Passport;
                searchedEmployee.PhoneNumber = newEmployee.PhoneNumber;
                searchedEmployee.Id = newEmployee.Id;
                searchedEmployee.Position = newEmployee.Position;
                searchedEmployee.Salary = newEmployee.Salary;
                searchedEmployee.Age = newEmployee.Age;
        }
        

        public List<Employee>? GetEmployeesByFilter(Func<Employee, bool> filter = null)
        {
            if (filter is null)
            {
                return _employeeStorage.Employees.ToList();
            }
            var selectedEmployees = _employeeStorage.Employees.Where(empl => filter(empl))
                .ToList();
            return selectedEmployees;
        }
    }
}