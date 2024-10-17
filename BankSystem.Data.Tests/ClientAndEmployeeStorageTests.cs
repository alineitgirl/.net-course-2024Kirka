using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BankSystem.App.Services;
using BankSystem.Data.DbContext;
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
            var testDataGenerator = new TestDataGenerator();
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var clientStorage = new ClientStorage(new BankSystemDbContext());
            foreach (var client in listOfClients)
            {
                clientStorage.Add(client);
            }
            Client newClient = new Client()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                DateOfBirth = DateTime.Now.ToUniversalTime(),
                Adress = "city1",
                Passport = "122ffj",
                Age = 18,
                Id = Guid.NewGuid(),
                PhoneNumber = "70001"
            };
            List<Account> accounts = new List<Account>()
            {
                new Account {Amount = 1234.5, CurrencyName = "USD"}
            };
            Client oldClient = new Client()
            {
                FirstName = "Petr",
                LastName = "Petrov",
                DateOfBirth = DateTime.Now.ToUniversalTime(),
                Adress = "city1",
                Passport = "12jjf2",
                Age = 87,
                Id = Guid.NewGuid(),
                PhoneNumber = "70002"
            };

            //Act
             clientStorage.Add(newClient);
             clientStorage.Add(oldClient);
             clientStorage.Update(newClient.Id, oldClient);
            clientStorage.Delete(oldClient.Id);
            clientStorage.AddAccount(newClient.Id, accounts[0]);
            clientStorage.UpdateAccount(newClient.Id, accounts[0], new Account
            {
                 Amount = 123.45, 
                 CurrencyName = "EUR"
            });


            //Assert
            Assert.Empty(clientStorage.
                GetByFilter(cl => cl.Age == 18,
                    c => c.FirstName, c => c.Passport, 1, 1));
            Assert.IsType<List<Client>>(clientStorage.GetByFilter(cl => cl.Accounts.Contains(new Account {
               Amount = 1234.5, 
               CurrencyName = "USD"
            }), c => c.PhoneNumber, c => c.FirstName, 1, 1).ToList());
        }

        [Fact]
        public void TestEmployeeStorage()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
            foreach (var employee in listOfEmployees)
            {
                employeeStorage.Add(employee);
            }   
            var newEmployee = new Employee()
            {
                FirstName = "Natalya",
                LastName = "Ivanova",
                DateOfBirth = DateTime.Now.ToUniversalTime(),
                Adress = "New-York",
                Passport = "123abc",
                PhoneNumber = "345189",
                Id = Guid.NewGuid(),
                Age = 18,
                Contract = "12345",
                Department = "HR-отдел",
                Position = "HR-менеджер",
                Salary = 1234.5,
            };
            var oldEmployee = new Employee()
            {
                FirstName = "Oleg",
                LastName = "Scvortsov",
                DateOfBirth = DateTime.Now.ToUniversalTime(),
                Adress = "Tiraspol",
                Passport = "123abcd",
                PhoneNumber = "234567",
                Id = Guid.NewGuid(),
                Age = 74,
                Contract = "12345",
                Department = "HR-отдел",
                Position = "HR-менеджер",
                Salary = 12346.5,
            };
            
            //Act
            employeeStorage.Add(oldEmployee);
            employeeStorage.Add(newEmployee);
            employeeStorage.Update(oldEmployee.Id, newEmployee);
            employeeStorage.Delete(newEmployee.Id);
            var employeesLivingInNewYork = employeeStorage.GetByFilter(empl =>
                empl.Adress == "Tiraspol", c => c.FirstName, c => c.PhoneNumber, 1, 1).ToList();

            //Assert
            Assert.Empty(employeeStorage.GetByFilter(empl => empl.Equals(newEmployee),
                c => c.FirstName, c => c.PhoneNumber, 1, 1));
            Assert.Empty(employeesLivingInNewYork);
        }
    }
}

