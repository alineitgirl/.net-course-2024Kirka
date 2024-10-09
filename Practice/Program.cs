using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
            Employee employee = new Employee
            {
                FirstName = "Иван",
                LastName = "Иванов",
                DateOfBirth = new DateTime(1979, 12, 13),
                Adress = "г. Тирасполь",
                Passport = "I-ПР345678",
                PhoneNumber = "077845632",
                Position = "backend-разработчик",
                Department = "IT-отдел",
                Salary = 8575,
            };
                
            Console.WriteLine(employee.ToString());
            ChangeEmployeesContract(employee);
            Console.WriteLine(employee.ToString());
            
            Currency currency = new Currency {
                CurrencyCode = "EUR", 
                CurrencyName = "Евро", 
                CurrencySymbol = "E"};
            
            Console.WriteLine(currency.ToString());
            UpdateCurrency(currency);
            Console.WriteLine(currency.ToString());
            
            //castingandtypeconversion
            BankService<Person> bankService = new BankService<Person>();
            Console.WriteLine(bankService.CountOwnersSalary(12345667, 34567, 10) == -1
                ? "Ошибка: Количество владельцев банка не может быть равным 0"
                : $"Зарплата каждого владельца банка: {bankService.CountOwnersSalary(12345667, 34567, 10)}");
            
            //list and dictionary types
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            var listOfClients =  testDataGenerator.GenerateListOfClients(1000);
            var dictionaryOfClients = testDataGenerator.GenerateDictionaryOfClients();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(1000);
            SearchClientInList(1000, listOfClients);
            SearchClientInDictionary(1000, dictionaryOfClients);
            SearchClientByAge(listOfClients);
            SearchEmployeeWithMinSalary(listOfEmployees);
            CompareSearchingByKeyAndLinq(1000, dictionaryOfClients);
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

        public static void SearchClientInList(int iterations, List<Client> listOfClients)
        {
            TimeSpan linqTotalTime = new TimeSpan();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                sw.Restart();
                var searchedClientInList= listOfClients.FirstOrDefault(cl => cl.PhoneNumber == "76657");
                sw.Stop();
                linqTotalTime += sw.Elapsed;
            }
            Console.WriteLine($"Среднее время поиска с помощью методов расширения: {linqTotalTime.TotalMilliseconds/iterations:F8} ms \n\n\n");
        }

        public static void SearchClientInDictionary(int iterations, Dictionary<string, string> dictionaryOfClients)
        {
            TimeSpan linqTotalTime = new TimeSpan();
            var sw = Stopwatch.StartNew();
            var searchedClientInDictionary = new KeyValuePair<string, string>();
            for (int i = 0; i < iterations; i++)
            {
                sw.Restart();
                searchedClientInDictionary = dictionaryOfClients.
                    FirstOrDefault(cl => cl.Key == "+75999");
                sw.Stop();
                linqTotalTime += sw.Elapsed;
            }
            Console.WriteLine($"Поиск завершен. Номер телефона клиента: {searchedClientInDictionary.Key}, его имя {searchedClientInDictionary.Value}");
            Console.WriteLine($"Затраченное время на поиск клиента в словаре: {linqTotalTime.TotalMilliseconds/iterations:F8}\n\n\n");
        }

        public static void  SearchClientByAge(List<Client> listOfClients)
        {
            var searchedClientsByAge = listOfClients.Where(cl => cl.Age < 22);
            foreach (var client in searchedClientsByAge)
            {
                Console.WriteLine(client.ToString());
            }
            Console.WriteLine("\n\n\n\n");
        }

        public static void SearchEmployeeWithMinSalary(List<Employee> employees)
        {
            var MinSalary = employees.Min(empl => empl.Salary);
            var employeeWithMinSalary = employees.Find(empl => empl.Salary == MinSalary);
            Console.WriteLine($"Минимальная заработная плата: {employeeWithMinSalary.ToString()}");
        }

        public static void CompareSearchingByKeyAndLinq(int iterations, Dictionary<string, string> dictionaryOfClients)
        {
            TimeSpan linqTotalTime = new TimeSpan();
            TimeSpan keyTotalTime = new TimeSpan();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                sw.Restart();
                var searchedInDictionaryByLinq = dictionaryOfClients.LastOrDefault(cl => cl.Key == "+76993");
                sw.Stop();
                linqTotalTime += sw.Elapsed;
                
                sw.Restart(); 
                var searchedInDictionaryByKey = dictionaryOfClients["+76993"];
                sw.Stop();
                keyTotalTime += sw.Elapsed;
            }
            Console.WriteLine($"Поиск с помощью методов расширения LINQ: {linqTotalTime.TotalMilliseconds/iterations:F8}");
            Console.WriteLine($"Поиск значения по ключу: {keyTotalTime.TotalMilliseconds/iterations:F8}");
        }
    }
} 