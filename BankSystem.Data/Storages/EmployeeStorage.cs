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

        public void UpdateEmployeeFromStorage(Employee employee)
        {
            if (_employees.Contains(employee))
            {
                int indexOfEmployee = _employees.FindIndex(empl => empl.PhoneNumber == employee.PhoneNumber);
                if (indexOfEmployee != -1)
                {
                    _employees[indexOfEmployee] = employee;
                }
            }
            else
            {
                AddEmployeeToStorage(employee);
            }
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

        public Employee? SearchEmployeeInStorage(Employee employee)
        {
            return _employees.FirstOrDefault(empl => empl.PhoneNumber == employee.PhoneNumber);
        }
        
        public IEnumerable<Employee> FindTheYoungestEmployee()
        {
            var minAge = _employees.Min(empl => empl.Age);
            var employeesWithMinAge = _employees.Where(empl => empl.Age == minAge);
            return employeesWithMinAge;
        }

        public IEnumerable<Employee> FindTheOldestEmployee()
        {
            var maxAge = _employees.Max(empl => empl.Age);
            var employeesWithMaxAge = _employees.Where(empl => empl.Age == maxAge);
            return employeesWithMaxAge;
        }

        public double CalculateAverageAgeOfEmployees()
        {
           return  _employees.Average(empl => empl.Age);
        }
    }
}