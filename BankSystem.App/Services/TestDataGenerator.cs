using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class TestDataGenerator
    {
        private string _generatePhoneNumber(int i)
        {
            return ((i + 10000) * 7).ToString();
        }
        
        public List<Client> GenerateListOfClients(int countOfClients)
        {

            var preferences = new List<string>()
                {"не звонить", 
                    "не отправлять рассылки", 
                    "капризный", 
                    "доброжелательный"};
            
            var clients = new List<Client>();
            Random rand = new Random();
            for (int i = 0; i < countOfClients; i++)
            {
                clients.Add(new Client
                {
                    FirstName = "firstName_" + i, 
                    LastName = "lastName_" + i, 
                    DateOfBirth = DateTime.Today, 
                    Adress =  i + "adress", 
                    Passport = "passport_" + i,
                    PhoneNumber = _generatePhoneNumber(i),
                    AccountBalance =  (rand.Next(0, 1899305)) / 3.0,
                    Age = rand.Next(18, 88)
                }
                );
            }

            return clients;
        }

        public Dictionary<string, string> GenerateDictionaryOfClients()
        {
            var alphabet = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";
            var symbols = "!@#$%^&*()_";
            Random rand = new Random();
            var clients = new Dictionary<string, string>();
            for (int i = 0; i < 1000; i++)
            {
                clients.Add("+" + _generatePhoneNumber(i), string.Concat(alphabet[rand.Next(0, alphabet.Length)], 
                    symbols[rand.Next(0, symbols.Length)], 
                    alphabet[rand.Next(0, alphabet.Length)]));
            }

            return clients;
        }

        public List<Employee> GenerateListOfEmployees(int countOfEmployees)
        {
            var  positions= new List<string>()
            {
                "fontend- разработчик", 
                "backend-разработчик", 
                "стажер", 
                "техподдержка", 
                "техлид", 
                "mobile-разработчик",
                "бухгалтер", 
                "менеджер по продажам"
            };
            
            var departments = new List<string>()
            {
                "IT-отдел", "Финансовый отдел", "Отдел продаж"
            };
            var employees = new List<Employee>();
            Random rand = new Random();
            for (int i = 0; i < countOfEmployees; i++)
            {

                employees.Add(new Employee
                {
                    FirstName = "firstName_" + i, 
                    LastName = "lastName_" + i, 
                    DateOfBirth = DateTime.Today, 
                    Adress = i + "adress", 
                    Passport = "passport_" + i,
                    PhoneNumber = _generatePhoneNumber(i),
                    Position =  positions[rand.Next(0, positions.Count)],
                    Department = departments[rand.Next(0, departments.Count)],
                    Salary = rand.Next(4500, 20000) / 3.0, 
                    Age = rand.Next(20, 75)
                }
                );
            }

            return employees;
        }

        public List<Account> GenerateAccountOfClient(int numberOfClients)
        {
            Random rand = new Random();
            List<Account> accounts = new List<Account>();
            List<string> currencies = new List<string>() { "RUP", "USD", "EUR", "MDL", "UAH", "RUB"};
            for (int i = 0; i < numberOfClients + 1; i++)
            {
                accounts.Add(new Account
                {
                    Currency = currencies[rand.Next(currencies.Count)],  
                    Amount = numberOfClients * 9 + 12345
                });
            }

            return accounts;
        }
        
        public Dictionary<Client, List<Account>> GenerateClientsDictionary(int countOfClients)
        {
            var dictionaryOfClients = new Dictionary<Client, List<Account>>();
            List<Client> clients = GenerateListOfClients(countOfClients);
            for (int i = 0; i < countOfClients; i++)
            {
                var listOfAccounts = GenerateAccountOfClient(i);
                dictionaryOfClients.Add(clients[i], listOfAccounts);
            }
            return dictionaryOfClients;
        }
    }
}