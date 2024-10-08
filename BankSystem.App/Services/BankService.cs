using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankService<T> where T : Person 
    {
        private List<Person> _blackList = new List<Person>();
        public int? CountOwnersSalary(decimal profit, decimal expenses, int numberOfEmployees)
        {
            if (numberOfEmployees != 0)
            {
                return (int) (profit - expenses) / numberOfEmployees;
            }
            else return -1;
        }

        public void AddBonus(Person person)
        {
            if (person is Client client)
            {
                client.AccountBalance += client.AccountBalance * 0.03;
            }

            if (person is Employee employee)
            {
                employee.Salary += employee.Salary * 0.1;
            }
        }

        public void AddToBlackList(T person) 
        {
            _blackList.Add(person);
        }

        public bool IsPersonInBlackList(T person)
        {
            return _blackList.Contains(person);
        }
    }
}