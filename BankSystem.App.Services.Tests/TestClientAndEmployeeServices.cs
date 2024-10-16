
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Data.DbContext;
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
            var testDataGenerator = new TestDataGenerator();
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()));
            foreach (var client in listOfClients)
            {
                clientService.AddClient(client);
            }
            
            //Act    
            var newClient = new Client {FirstName = "Олег", LastName = "Скворцов", Age = 16};

            //Assert
            Assert.Throws<AgeOutOfRangeException>(() => clientService.AddClient(newClient));
        }

        [Fact]
        public void AddClient_WithInvalidPassport_ThrowsException()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()));
            foreach (var client in listOfClients)
            {
                clientService.AddClient(client);
            }
            
            //Act
            var newClient = new Client {FirstName = "Олег", LastName = "Скворцов", Age = 20};

            //Assert
            Assert.Throws<NoInfoAboutPassportNumberException>(() => clientService.AddClient(newClient));
        }

        [Fact]
        public void AddUpdateAndGetClientsByFilter_PositivTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()));
            foreach (var client in listOfClients)
            {
                clientService.AddClient(client);
            }
            
            var newClient = new Client
                {
                    FirstName = "Олег", LastName = "Скворцов", Age = 20, Passport = "123456", PhoneNumber = "999999",
                    DateOfBirth = new DateTime(2005, 12, 15).ToUniversalTime()
                };
            var clientToSearch = new Client
                {
                    FirstName = "Олег", 
                    LastName = "Скворцов", 
                    Passport = "123456", 
                    PhoneNumber = "999999",
                    DateOfBirth = new DateTime(2005, 12, 15)
                };
            
            //Act 
            clientService.AddClient(newClient);
            clientService.AddNewAccountToClient(newClient.Id, new Account {Amount = 123, CurrencyName = "EUR"});
            clientService.UpdateAddedAccountOfClient(newClient.Id,new Account {Amount = 123, CurrencyName = "EUR"}, 
                new Account {Amount = 444, CurrencyName = "RUP" });
            var clientsWithSameName  = clientService.GetByFilter(cl =>
                cl.FirstName == clientToSearch.FirstName && cl.LastName == clientToSearch.LastName,
                c => c.Age, g => g.DateOfBirth, 1, 1).ToList();
            var clientWithSamePhoneNumber = clientService.GetByFilter(cl =>
                cl.PhoneNumber == clientToSearch.PhoneNumber, c => c.PhoneNumber, c => c.Age, 1, 1).ToList();
            var clientWithSamePassportNumber = clientService.GetByFilter(cl =>
                cl.Passport == clientToSearch.Passport, c => c.Age,  c => c.Passport, 1, 1).ToList();

            //Assert
            Assert.Single(clientsWithSameName);
            Assert.Single(clientWithSamePhoneNumber);
            Assert.Single(clientWithSamePassportNumber);
        }
        
        [Fact]
        public void AddEmployee_WithInvalidAge_ThrowsException()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var employeeService = new EmployeeService(new EmployeeStorage(new BankSystemDbContext()));
            foreach (var employee in listOfEmployees)
            {
                employeeService.AddEmployee(employee);
            }

            
            //Act
            var newEmployee = new Employee()
            {
                FirstName = "Petr",
                LastName = "Petrov",
                Age = 16
            };

            //Assert
            Assert.Throws<AgeOutOfRangeException>(() => employeeService.AddEmployee(newEmployee));
        }

        [Fact]
        public void AddEmployee_WithInvalidPasport_ThrowsException()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var employeeService = new EmployeeService(new EmployeeStorage(new BankSystemDbContext()));
            foreach (var employee in listOfEmployees)
            {
                employeeService.AddEmployee(employee);
            }
            
            
            //Act
            var newEmployee = new Employee()
            {
                FirstName = "Olga",
                LastName = "Ponomareva",
                Age = 22,
            };

            //Assert
            Assert.Throws<NoInfoAboutPassportNumberException>(() => employeeService.AddEmployee(newEmployee));
        }

        [Fact]
        public void AddUpdateAndGetEmployeesByFilter_PositivTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var employeeService = new EmployeeService(new EmployeeStorage(new BankSystemDbContext()));
            foreach (var employee in listOfEmployees)
            {
                employeeService.AddEmployee(employee);
            }
            
            Employee newEmployee = new Employee
            {
                FirstName = "Александр",
                LastName =  "Пономарев",
                DateOfBirth = new DateTime(2005, 12, 15).ToUniversalTime(),
                Passport = "123456",
                PhoneNumber = "99999",
                Age = 22
            };
            Employee employeeToSearch = new Employee
            {
                FirstName = "Александр",
                LastName = "Пономарев",
                DateOfBirth = new DateTime(2005, 12, 15).ToUniversalTime(),
                Passport = "123456",
                PhoneNumber = "99999",
                Age = 22
            };


            //Act
            employeeService.AddEmployee(newEmployee);
            var employeeToUpdate = new Employee
            {
                FirstName = "",
                LastName = "",
                DateOfBirth = new DateTime(2004, 9, 13).ToUniversalTime(),
                Passport = "123456",
                PhoneNumber = "999999",
                Position = "junior-dev",
                Department = "IT-отдел"
            };
            
            var employeeByName = employeeService.GetByFilter(empl =>
                empl.FirstName == employeeToSearch.FirstName && empl.LastName == employeeToSearch.LastName,
                c => c.Age, g => g.Salary, 1, 1).ToList();
            employeeService.UpdateEmployee(newEmployee.Id, employeeToUpdate.FirstName, employeeToUpdate.LastName,
                employeeToUpdate.DateOfBirth, employeeToUpdate.PhoneNumber, employeeToUpdate.Passport,
                employeeToUpdate.Adress, employeeToUpdate.Age, employeeToUpdate.Position,
                employeeToUpdate.Salary, employeeToUpdate.Department);


            //Assert
            Assert.Single(employeeByName);
            Assert.Empty(employeeService.GetByFilter(empl => empl.FirstName == "Александр",
                c => c.Age, g => g.Salary, 1, 1));
        }
    }
}

