using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.App.Services.Tests
{
    public class TestClientAndEmployeeServices
    {
        [Fact]
        public void AddClient_WithInvalidAge_ThrowsException()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService(new ClientStorage(testDataGenerator.GenerateClientsDictionary(10)));
            
            //Act
            var newClient = new KeyValuePair<Client, List<Account>>(
                new Client {FirstName = "Олег", LastName = "Скворцов", Age = 16},
                new List<Account>
                {
                    new Account { Amount = 13245, Currency = "RUP" },
                    new Account {Amount = 9999, Currency = "USD"}
                });

            //Assert
            Assert.Throws<AgeOutOfRangeException>(() => clientService.AddClientToService(newClient));
        }

        [Fact]
        public void AddClient_WithInvalidPassport_ThrowsException()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService(new ClientStorage(testDataGenerator.GenerateClientsDictionary(10)));
            
            //Act
            var newClient = new KeyValuePair<Client, List<Account>>(
                new Client {FirstName = "Олег", LastName = "Скворцов", Age = 20},
                new List<Account>
                {
                    new Account { Amount = 13245, Currency = "RUP" },
                    new Account {Amount = 9999, Currency = "USD"}
                });

            //Assert
            Assert.Throws<NoInfoAboutPassportNumberException>(() => clientService.AddClientToService(newClient));
        }

        [Fact]
        public void AddUpdateAndGetClientsByFilter_PositivTest()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService(new ClientStorage(testDataGenerator.GenerateClientsDictionary(10)));
            var newClient = new KeyValuePair<Client, List<Account>>(
                new Client
                {
                    FirstName = "Олег", LastName = "Скворцов", Age = 20, Passport = "123456", PhoneNumber = "999999",
                    DateOfBirth = new DateTime(2005, 12, 15)
                },
                new List<Account>());
            var clientToSearch = new KeyValuePair<Client, List<Account>>(
                new Client
                {
                    FirstName = "Олег", 
                    LastName = "Скворцов", 
                    Passport = "123456", 
                    PhoneNumber = "999999",
                    DateOfBirth = new DateTime(2005, 12, 15)
                },
                new List<Account>());
            
            //Act 
            clientService.AddClientToService(newClient);
            clientService.AddNewAccountToClient(newClient, new Account {Amount = 123, Currency = "EUR"});
            clientService.UpdateAddedAccountOfClient(newClient,new Account {Amount = 123, Currency = "EUR"}, 
                new Account {Amount = 444, Currency = "RUP"});
            var clientWithSameName  = clientService.GetClientsByFilter(cl =>
                cl.FirstName == clientToSearch.Key.FirstName && cl.LastName == clientToSearch.Key.LastName);
            var clientWithSamePhoneNumber = clientService.GetClientsByFilter(cl =>
                cl.PhoneNumber == clientToSearch.Key.PhoneNumber);
            var clientWithSamePassportNumber = clientService.GetClientsByFilter(cl =>
                cl.Passport == clientToSearch.Key.Passport);
            var clientWithAllFilters = clientService.GetClientsByFilter(cl =>
                cl.Equals(clientToSearch.Key) && cl.DateOfBirth >= new DateTime(2000, 12, 31) && 
                cl.DateOfBirth <= new DateTime(2010, 12, 31));

            //Assert
            Assert.Single(clientWithSameName);
            Assert.Single(clientWithSamePhoneNumber);
            Assert.Single(clientWithSamePassportNumber);
            Assert.Single(clientWithAllFilters);
        }
        
        [Fact]
        public void AddEmployee_WithInvalidAge_ThrowsException()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeService employeeService = new EmployeeService(new EmployeeStorage(testDataGenerator.GenerateListOfEmployees(10)));
            
            //Act
            var newEmployee = new Employee()
            {
                FirstName = "Petr",
                LastName = "Petrov",
                Age = 16
            };

            //Assert
            Assert.Throws<AgeOutOfRangeException>(() => employeeService.AddEmployeeToService(newEmployee));
        }

        [Fact]
        public void AddEmployee_WithInvalidPasport_ThrowsException()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeService employeeService = new EmployeeService(new EmployeeStorage(testDataGenerator.GenerateListOfEmployees(10)));
            
            //Act
            var newEmployee = new Employee()
            {
                FirstName = "Olga",
                LastName = "Ponomareva",
                Age = 22,
            };

            //Assert
            Assert.Throws<NoInfoAboutPassportNumberException>(() => employeeService.AddEmployeeToService(newEmployee));
        }

        [Fact]
        public void AddUpdateAndGetEmployeesByFilter_PositivTest()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
            EmployeeService employeeService = new EmployeeService(new EmployeeStorage(testDataGenerator.GenerateListOfEmployees(10)));
            Employee newEmployee = new Employee
            {
                FirstName = "Александр",
                LastName =  "Пономарев",
                DateOfBirth = new DateTime(2005, 12, 15),
                Passport = "123456",
                PhoneNumber = "99999",
                Age = 22
            };
            Employee employeeToSearch = new Employee
            {
                FirstName = "Александр",
                LastName = "Пономарев",
                DateOfBirth = new DateTime(2005, 12, 15),
                Passport = "123456",
                PhoneNumber = "99999",
                Age = 22
            };


            //Act
            employeeService.AddEmployeeToService(newEmployee);
            var employeeToUpdate = new Employee
            {
                FirstName = "",
                LastName = "",
                DateOfBirth = new DateTime(2004, 9, 13),
                Passport = "123456",
                PhoneNumber = "999999"
            };
            
            var employeeByName = employeeService.GetEmployeesByFilter(empl =>
                empl.FirstName == employeeToSearch.FirstName && empl.LastName == employeeToSearch.LastName);
            var employeeWithAllFilters = employeeService.GetEmployeesByFilter(empl =>
                empl.Equals(employeeToSearch) && empl.DateOfBirth >= new DateTime(2000, 12, 31)
                                              && empl.DateOfBirth <= new DateTime(2024, 10, 04));
            var allEmployees = employeeService.GetEmployeesByFilter();
            employeeService.UpdateEmployeeFromService(newEmployee, employeeToUpdate);


            //Assert
            Assert.Single(employeeByName);
            Assert.Single(employeeWithAllFilters);
            Assert.Equal(11, allEmployees.Count);
            Assert.Empty(employeeService.GetEmployeesByFilter(empl => empl.FirstName == "Александр"));
        }
    }
}

