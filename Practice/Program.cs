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
                "I-ПР345678", 123456789, "backend-разработчик", "IT-отдел", 8575, "junior");
            employee.Print();
            ChangeEmployeesContract(employee);
            employee.Print();
            
            Currency currency = new Currency("EUR", "Евро", "E", (decimal)18.75);
            currency.Print();
            UpdateCurrency(currency);
            currency.Print();
            
            //castingandtypeconversion
            BankService bankService = new BankService(987456.987m, 235678.345m, new List<Employee>(){employee});
            Console.WriteLine(bankService.CountOwnersSalary() is null
                ? "Ошибка: Количество владельцев банка не может быть равным 0"
                : $"Зарплата каждого владельца банка: {bankService.CountOwnersSalary()}");
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
            currency.ExchangeRate = (decimal) 16.35;
        }
    }
} 