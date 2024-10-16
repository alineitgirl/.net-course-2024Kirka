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
             clientStorage.Update(newClient.Id, oldClient.FirstName, oldClient.LastName, oldClient.PhoneNumber, 
                 oldClient.Passport, oldClient.DateOfBirth, oldClient.Adress, oldClient.Age);
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
                    c => c.FirstName, null, 1, 1));
            Assert.Empty(clientStorage.GetByFilter(cl => cl.Accounts.Contains(new Account {
               Amount = 1234.5, 
               CurrencyName = "USD"
            }), null, null, 1, 1));
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
                DateOfBirth = DateTime.Now,
                Adress = "New-York",
                Passport = "123abc",
                PhoneNumber = "70000",
                Id = Guid.NewGuid(),
                Age = 18
            };
            var oldEmployee = new Employee()
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
            employeeStorage.Add(newEmployee);
            employeeStorage.Update(oldEmployee.Id, newEmployee.FirstName, newEmployee.LastName, newEmployee.DateOfBirth,
                newEmployee.PhoneNumber, newEmployee.Passport, newEmployee.Adress, newEmployee.Age,
                newEmployee.Position, newEmployee.Salary, newEmployee.Department);
            employeeStorage.Delete(newEmployee.Id);
            var employeesLivingInNewYork = employeeStorage.GetByFilter(empl =>
                empl.Adress == "Tiraspol", null, null, 1, 1).ToList();

            //Assert
            Assert.Empty(employeeStorage.GetByFilter(empl => empl.Equals(newEmployee),
                null, null, 1, 1));
            Assert.Empty(employeesLivingInNewYork);
        }
    }
}

