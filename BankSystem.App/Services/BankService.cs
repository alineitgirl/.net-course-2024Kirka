using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankService
    {
        private decimal Profit { get; set; } = 0;
        private decimal Expenses { get; set; } = 0;
        private List<Employee> numberOfEmployees = null;

        public BankService()
        {
            
        }

        public BankService(decimal profit, decimal expenses, List<Employee> employees)
        {
            this.Profit = profit;
            this.Expenses = expenses;
            if (employees != null)
            {
                numberOfEmployees = new List<Employee>();
                numberOfEmployees.AddRange(employees);
            }
        }

        public int? CountOwnersSalary()
        {
            if (numberOfEmployees.Count != 0)
            {
                return (int) (this.Profit - this.Expenses) / numberOfEmployees.Count;
            }
            else return null;
        }
        
        //компилятор выдает ошибку: горизонтальные преобразования в с# классов невозможны
        /*public void ConvertToEmployee(Client client)
        {
            if (client is Employee employee)
            {
                Console.WriteLine("Преобразование прошло успешно.");
            }
            else
            {
                Console.WriteLine("Преобразование не допустимо.");
            }
        }*/
    }
}