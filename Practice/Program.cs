using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            //value and reference types
            Employee employee = new Employee("Иван", "Иванов", new DateTime(1979, 12, 13), "г. Тирасполь",
                "I-ПР345678", "077845632", "backend-разработчик", "IT-отдел", 8575, "junior");
            Console.WriteLine(employee.ToString());
            ChangeEmployeesContract(employee);
            Console.WriteLine(employee.ToString());
            
            Currency currency = new Currency("EUR", "Евро", "E");
            Console.WriteLine(currency.ToString());
            UpdateCurrency(currency);
            Console.WriteLine(currency.ToString());
            
            //castingandtypeconversion
            BankService bankService = new BankService();
            Console.WriteLine(bankService.CountOwnersSalary(12345667, 34567, 10) == -1
                ? "Ошибка: Количество владельцев банка не может быть равным 0"
                : $"Зарплата каждого владельца банка: {bankService.CountOwnersSalary(12345667, 34567, 10)}");
        }

        public static void ChangeEmployeesContract(Employee employee)
        {
            employee.Contract = "Контракт успешно обновлён";
        }

        public static void UpdateCurrency(Currency currency)
        {
            currency.CurrencyCode = "USD";
            currency.CurrencyName = "Доллар США";
            currency.CurrencySymbol = "$";
        }
    }
} 