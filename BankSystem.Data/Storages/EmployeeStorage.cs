using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage
    {
        private List<Employee> _employees;

        public EmployeeStorage(List<Employee> employees)
        {
            _employees = employees;
        }

        public bool AddEmployeeToStorage(Employee employee)
        {
            if (!(_employees.Contains(employee)))
            {
                _employees.Add(employee);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateEmployeeFromStorage(Employee oldEmployee, Employee newEmployee)
        {
            if (!(_employees.Contains(oldEmployee)))
            {
                AddEmployeeToStorage(oldEmployee);
            }
            var searchedEmployee = _employees.FirstOrDefault(empl => empl.Equals(oldEmployee));
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

        public bool DeleteEmployeeFromStorage(Employee employee)
        {
            if ((_employees.Contains(employee)))
            {
                _employees.Remove(employee);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Employee> GetEmployeesByFilter(Func<Employee, bool> filter = null)
        {
            if (filter is null)
            {
                return _employees;
            }
            var selectedEmployees = _employees.Where(empl => filter(empl))
                .ToList();
            return selectedEmployees;
        }
        public Employee FindTheYoungestEmployee()
        {
            var minAge = _employees.Min(empl => empl.Age);
            var employeesWithMinAge = _employees.Where(empl => empl.Age == minAge).ToList();
            return employeesWithMinAge.FirstOrDefault(empl => empl.Age == minAge);
        }

        public Employee FindTheOldestEmployee()
        {
            var maxAge = _employees.Max(empl => empl.Age);
            var employeesWithMaxAge = _employees.Where(empl => empl.Age == maxAge).ToList();
            return employeesWithMaxAge.FirstOrDefault(empl => empl.Age == maxAge);
        }

        public double CalculateAverageAgeOfEmployees()
        {
           return  _employees.Average(empl => empl.Age);
        }
    }
}