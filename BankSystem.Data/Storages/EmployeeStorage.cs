using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private List<Employee> _employees;

        public EmployeeStorage(List<Employee> employees)
        {
            _employees = employees;
        }

        public void Add(Employee employee)
        {
            if (!(_employees.Contains(employee)))
            {
                _employees.Add(employee);
            }
        }

        public void Update(Employee oldEmployee, Employee employeeToUpdate)
        {
            if (!(_employees.Contains(oldEmployee)))
            {
                Add(oldEmployee);
            }

            var searchedEmployee = _employees.FirstOrDefault(empl => empl.Equals(oldEmployee));
            if (searchedEmployee != null)
            {
                searchedEmployee.FirstName= employeeToUpdate.FirstName;
                searchedEmployee.LastName = employeeToUpdate.LastName;
                searchedEmployee.DateOfBirth = employeeToUpdate.DateOfBirth;
                searchedEmployee.Adress = employeeToUpdate.Adress;
                searchedEmployee.Passport = employeeToUpdate.Passport;
                searchedEmployee.PhoneNumber = employeeToUpdate.PhoneNumber;
                searchedEmployee.Id = employeeToUpdate.Id;
                searchedEmployee.Position = employeeToUpdate.Position;
                searchedEmployee.Salary = employeeToUpdate.Salary;
                searchedEmployee.Age = employeeToUpdate.Age;
            }
        }

        public void Delete(Employee employee)
        {
            if ((_employees.Contains(employee)))
            {
                _employees.Remove(employee);
            }
        }

        public IEnumerable<Employee> Get(Func<Employee, bool> filter = null)
        {
            if (filter is null)
            {
                return _employees;
            }

            var selectedEmployees = _employees.Where(filter);
            return selectedEmployees;
        }
        
    }
}