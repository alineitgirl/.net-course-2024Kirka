using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;

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
            bool resultOfAdding = clientStorage.AddClientToStorage(new KeyValuePair<Client, List<Account>>(newClient, accounts));
            clientStorage.AddClientToStorage(new KeyValuePair<Client, List<Account>>(oldClient, accounts));
            newClient.PhoneNumber = "11111";
            clientStorage.UpdateClientFromStorage(new KeyValuePair<Client, List<Account>>(newClient, accounts));
            newClient.PhoneNumber = "70001";
            bool resultOfDeleting =
                clientStorage.DeleteClientFromStorage(new KeyValuePair<Client, List<Account>>(newClient, accounts));
            var youngestClient = clientStorage.FindTheYoungestClient();
            var oldestClient = clientStorage.FindTheOldestClient();
            

            //Assert
            Assert.True(resultOfAdding);
            Assert.Single(clientStorage.GetClientsByFilter(client => client.Equals(newClient)));
            Assert.True(resultOfDeleting);
            Assert.True(youngestClient.Key.Age == 18);
            Assert.True(oldestClient.Key.Age == 87);
            Assert.InRange(clientStorage.CalculateAverageAgeOfClients(), 25, 60);
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
            bool resultOfAdding = employeeStorage.AddEmployeeToStorage(newEmployee);
            newEmployee.PhoneNumber = "00000";
            bool resultOfDeleting = employeeStorage.DeleteEmployeeFromStorage(newEmployee);
            employeeStorage.AddEmployeeToStorage(newEmployee);
            employeeStorage.AddEmployeeToStorage(oldEmployee);
            var youngestEmployee = employeeStorage.FindTheYoungestEmployee();
            var oldestEmployee = employeeStorage.FindTheOldestEmployee();

            //Assert
            Assert.True(resultOfAdding);
            Assert.True(resultOfDeleting);
            Assert.IsType<List<Employee>>(employeeStorage.GetEmployeesByFilter(empl => empl.Equals(newEmployee)));
            Assert.True(youngestEmployee.Age == 18);
            Assert.True(oldestEmployee.Age == 74);
            Assert.InRange(employeeStorage.CalculateAverageAgeOfEmployees(), 25, 60);
        }
    }
}

