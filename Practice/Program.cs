using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
            
            //list and dictionary types
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            var listOfClients =  testDataGenerator.GenerateListOfClients();
            var dictionaryOfClients = testDataGenerator.GenerateDictionaryOfClients();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees();

            var sw = Stopwatch.StartNew();
            var searchedClientInList = listOfClients.FirstOrDefault(cl => cl.PhoneNumber == "76657");
            sw.Stop();
            if (!(searchedClientInList is null))
                Console.WriteLine($"Клиент с номером телефона 76657 найден");
             else
                Console.WriteLine("Клиента с таким номером нет!");
            Console.WriteLine($"Затраченное время на поиск клиента в списке: {sw.Elapsed}\n\n\n");
            
            sw.Restart();
            var searchedClientInDictionary = dictionaryOfClients.
                FirstOrDefault(cl => cl.Key == "+75999");
            sw.Stop();
            Console.WriteLine($"Поиск завершен. Номер телефона клиента: {searchedClientInDictionary.Key}, его имя {searchedClientInDictionary.Value}");
            Console.WriteLine($"Затраченное время на поиск клиента в словаре: {sw.Elapsed}\n\n\n");
            
            
            var searchedClientsByAge = listOfClients.Where(cl => cl.Age < 22);
            foreach (var client in searchedClientsByAge)
            {
                Console.WriteLine(client.ToString());
            }
            Console.WriteLine("\n\n\n\n");

            var MinSalary = listOfEmployees.Min(empl => empl.Salary);
            var employeeWithMinSalary = listOfEmployees.Find(empl => empl.Salary == MinSalary);
            Console.WriteLine($"Минимальная заработная плата: {employeeWithMinSalary.ToString()}");
            
            sw.Restart();
            var searchedInDictionaryByLinq = dictionaryOfClients.LastOrDefault(cl => cl.Key == "+76993");
            sw.Stop();
            Console.WriteLine($"Поиск с помощью методов расширения LINQ: {sw.Elapsed}");
            sw.Restart();
            var searchedInDictionaryByKey = dictionaryOfClients["+76993"];
            sw.Stop();
            Console.WriteLine($"Поиск значения по ключу: {sw.Elapsed}");
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