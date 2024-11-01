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
        public async void TestClientsStorage()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var clientStorage = new ClientStorage(new BankSystemDbContext());
            foreach (var client in listOfClients)
            {
                await clientStorage.AddAsync(client);
            }
            Client newClient = new Client()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                DateOfBirth = DateTime.Now.ToUniversalTime(),
                Adress = "city1",
                Passport = "1289ffj",
                Age = 18,
                Id = Guid.NewGuid(),
                PhoneNumber = "5555643"
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
                Passport = "12hfyjui",
                Age = 87,
                Id = Guid.NewGuid(),
                PhoneNumber = "12345678"
            };

            //Act
            await clientStorage.AddAsync(newClient);
            await clientStorage.UpdateAsync(newClient.Id, oldClient);
            await clientStorage.DeleteAsync(oldClient.Id);
            await clientStorage.AddAccountAsync(newClient.Id, accounts[0]);
            await clientStorage.UpdateAccountAsync(newClient.Id, accounts[0], new Account
            {
                 Amount = 123.45, 
                 CurrencyName = "EUR"
            });
            var result1 = await clientStorage.GetByFilterAsync(cl => cl.Age == 18,
                c => c.OrderBy(cl => cl.Id), 1, 1);
            var result2 = await clientStorage.GetByFilterAsync(cl => cl.Accounts.Contains(new Account
            {
                Amount = 1234.5,
                CurrencyName = "USD"
            }), c => c.OrderBy(cl => cl.Id),1, 1);

            //Assert
            Assert.Empty(result1);
            Assert.IsType<List<Client>>(result2);
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
                employeeStorage.AddAsync(employee);
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
            employeeStorage.AddAsync(oldEmployee);
            employeeStorage.AddAsync(newEmployee);
            employeeStorage.UpdateAsync(oldEmployee.Id, newEmployee);
            employeeStorage.DeleteAsync(newEmployee.Id);
            var employeesLivingInNewYork = employeeStorage.GetByFilterAsync(empl =>
                empl.Adress == "Tiraspol", c => c.OrderBy(cl => cl.Adress), 1, 1)
                .Result.ToList();

            //Assert
            Assert.Empty(employeeStorage.GetByFilterAsync(empl => empl.Equals(newEmployee),
                c => c.OrderBy(cl => cl.PhoneNumber), 1, 1).Result.ToList());
            Assert.Empty(employeesLivingInNewYork);
        }
    }
}

