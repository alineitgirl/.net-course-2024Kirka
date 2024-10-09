using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests
{
    public class IEnumerableTests
    {
        [Fact]
        public void TestClientsStorage()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientStorage clientStorage = new ClientStorage(testDataGenerator.GenerateClientsDictionary(10)); 
            Client newClient = new Client()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                DateOfBirth = DateTime.Now,
                AccountBalance = 10000,
                Adress = "city1",
                Passport = "122ffj",
                Age = 18,
                Id = Guid.NewGuid(),
                PhoneNumber = "70001"
            };
            List<Account> accounts = new List<Account>()
            {
                new Account {Amount = 1234.5, Currency = "USD"},
            };
            Client oldClient = new Client()
            {
                FirstName = "Petr",
                LastName = "Petrov",
                DateOfBirth = DateTime.Now,
                AccountBalance = 10000,
                Adress = "city1",
                Passport = "12jjf2",
                Age = 87,
                Id = Guid.NewGuid(),
                PhoneNumber = "70002"
            };

            //Act
            clientStorage.Add(new KeyValuePair<Client, List<Account>>(newClient, accounts));
            clientStorage.Add(new KeyValuePair<Client, List<Account>>(oldClient, accounts));
            clientStorage.Update(new KeyValuePair<Client, List<Account>>(oldClient, accounts), 
                new KeyValuePair<Client, List<Account>>(newClient, accounts));
            clientStorage.Delete(new KeyValuePair<Client, List<Account>>(newClient, accounts));
            clientStorage.AddAccount(new KeyValuePair<Client, List<Account>>(newClient, accounts), 
                new Account { Amount =  123.45, Currency = "EUR"});
            clientStorage.UpdateAccount(new KeyValuePair<Client, List<Account>>(newClient, accounts),
            new Account {Amount = 123.45, Currency = "EUR"}, new Account {Amount = 134, Currency = "RUP"});
            clientStorage.DeleteAccount(new KeyValuePair<Client, List<Account>>(newClient, accounts), 
                new Account {Amount = 1234.5, Currency = "USD"});


            //Assert
            Assert.Empty(clientStorage.Get(cl => cl.Key.Age == 18));
            Assert.False(clientStorage.Get(cl => cl.Value.Contains(new Account
            {
                Amount = 1234.5, Currency = "USD"
            })).Any());
        }

        [Fact]
        public void TestEmployeeStorage()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeStorage employeeStorage = new EmployeeStorage(testDataGenerator.GenerateListOfEmployees(10));
            Employee newEmployee = new Employee()
            {
                FirstName = "Natalya",
                LastName = "Ivanova",
                DateOfBirth = DateTime.Now,
                Adress = "New-York",
                Passport = "123abc",
                PhoneNumber = "70000",
                Id = Guid.NewGuid(),
                Age = 18
            };
            Employee oldEmployee = new Employee()
            {
                FirstName = "Oleg",
                LastName = "Scvortsov",
                DateOfBirth = DateTime.Now,
                Adress = "Tiraspol",
                Passport = "123abc",
                PhoneNumber = "23456",
                Id = Guid.NewGuid(),
                Age = 74
            };
            
            //Act
            employeeStorage.Add(oldEmployee);
            employeeStorage.Update(oldEmployee, newEmployee);
            employeeStorage.Delete(newEmployee);
            var employeesLivingInNewYork = employeeStorage.Get(empl =>
                empl.Adress == "Tiraspol");

            //Assert
            Assert.Empty(employeeStorage.Get(empl => empl.Equals(oldEmployee)));
            Assert.Empty(employeesLivingInNewYork);
        }
    }
}

