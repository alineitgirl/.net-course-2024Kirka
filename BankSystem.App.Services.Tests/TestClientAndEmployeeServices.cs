
using AutoMapper;
using BankSystem.App.Dto;
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
            var mapper = new Mapper(new MapperConfiguration( cfg => cfg.CreateMap<Client, ClientDto>()));
            var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()), mapper);
            var cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;
            foreach (var client in listOfClients)
            {
                clientService.AddClientAsync(mapper.Map<Client, ClientDto>(client), token);
            }
            
            //Act    
            var newClient = new Client {FirstName = "Олег", LastName = "Скворцов", Age = 16};

            //Assert
            Assert.ThrowsAsync<AgeOutOfRangeException>(() => clientService.AddClientAsync(mapper.Map<Client, ClientDto>(newClient), token));
        }

        [Fact]
        public async void AddClient_WithInvalidPassport_ThrowsException()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Client, ClientDto>()));
            var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()), mapper);
            var cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;
            foreach (var client in listOfClients)
            {
                await clientService.AddClientAsync(mapper.Map<Client, ClientDto>(client), token);
            }
            
            //Act
            var newClient = new Client {FirstName = "Олег", LastName = "Скворцов", Age = 20};

            //Assert
            Assert.ThrowsAsync<NoInfoAboutPassportNumberException>(() => clientService.AddClientAsync(mapper.Map<Client, ClientDto>(newClient), token));
        }

        [Fact]
        public async Task AddUpdateAndGetClientsByFilter_PositivTest()
        {
            //Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var testDataGenerator = new TestDataGenerator();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Client, ClientDto>()));
            var listOfClients = testDataGenerator.GenerateListOfClients(10);
            var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()), mapper);
            foreach (var client in listOfClients)
            {
                await clientService.AddClientAsync(mapper.Map<Client, ClientDto>(client), token);
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
            await clientService.AddClientAsync(mapper.Map<Client, ClientDto>(newClient), token);
            await clientService.AddNewAccountToClientAsync(newClient.Id, new Account {Amount = 123, CurrencyName = "EUR"}, CancellationToken.None);
            await clientService.UpdateAddedAccountOfClientAsync(newClient.Id,new Account {Amount = 123, CurrencyName = "EUR"}, 
                new Account {Amount = 444, CurrencyName = "RUP" }, token);
            var clientsWithSameName  = await clientService.GetByFilterAsync(cl =>
                cl.FirstName == clientToSearch.FirstName && cl.LastName == clientToSearch.LastName,
                cl => cl.OrderBy(c => c.Id), 1, 1, token);
            var clientWithSamePhoneNumber = await clientService.GetByFilterAsync(cl =>
                cl.PhoneNumber == clientToSearch.PhoneNumber, c => 
                c.OrderBy(cl => cl.PhoneNumber), 1, 1, token);
            var clientWithSamePassportNumber = await clientService.GetByFilterAsync(cl =>
                cl.Passport == clientToSearch.Passport, c => c.OrderBy(cl => cl.PhoneNumber), 1, 1,
                token);

            //Assert
            Assert.Single(clientsWithSameName);
            Assert.Single(clientWithSamePhoneNumber);
            Assert.Single(clientWithSamePassportNumber);
        }
        
        [Fact]
        public async Task  AddEmployee_WithInvalidAge_ThrowsException()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDto>()));
            var cancellationToken = new CancellationTokenSource();
            var employeeService = new EmployeeService(new EmployeeStorage(new BankSystemDbContext()), mapper);
            foreach (var employee in listOfEmployees)
            {
                 await employeeService.AddEmployeeAsync(mapper.Map<Employee, EmployeeDto>(employee), cancellationToken.Token);
            }

            
            //Act
            var newEmployee = new Employee()
            {
                FirstName = "Petr",
                LastName = "Petrov",
                Age = 16
            };

            //Assert
            await Assert.ThrowsAsync<AgeOutOfRangeException>(() => employeeService.AddEmployeeAsync(mapper.Map<Employee, EmployeeDto>(newEmployee), cancellationToken.Token));
        }

        [Fact]
        public async void AddEmployee_WithInvalidPasport_ThrowsException()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDto>()));
            var cancellationToken = new CancellationTokenSource();
            var employeeService = new EmployeeService(new EmployeeStorage(new BankSystemDbContext()), mapper);
            foreach (var employee in listOfEmployees)
            {
                await employeeService.AddEmployeeAsync(mapper.Map<Employee, EmployeeDto>(employee), cancellationToken.Token);
            }
            
            
            //Act
            var newEmployee = new Employee()
            {
                FirstName = "Olga",
                LastName = "Ponomareva",
                Age = 22,
            };

            //Assert
            Assert.ThrowsAsync<NoInfoAboutPassportNumberException>(() => employeeService.AddEmployeeAsync(mapper.Map<Employee, EmployeeDto>(newEmployee), cancellationToken.Token));
        }

        [Fact]
        public async Task AddUpdateAndGetEmployeesByFilter_PositivTestcg()
        {
            //Arrange
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDto>())); 
            var cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;
            var testDataGenerator = new TestDataGenerator();
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var employeeService = new EmployeeService(new EmployeeStorage(new BankSystemDbContext()), mapper);
            foreach (var employee in listOfEmployees)
            {
                var employeeDto = mapper.Map<Employee, EmployeeDto>(employee);
                await employeeService.AddEmployeeAsync(employeeDto);
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
            
            await employeeService.AddEmployeeAsync(mapper.Map<Employee, EmployeeDto>(newEmployee));
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
            
            var employeeByName = await employeeService.GetByFilterAsync(empl =>
                empl.FirstName == employeeToSearch.FirstName && empl.LastName == employeeToSearch.LastName,
                c => c.OrderBy(cl => cl.Age),  1, 1);
            await employeeService.UpdateEmployeeAsync(newEmployee.Id, mapper.Map<Employee, EmployeeDto>(employeeToUpdate), token);
            var result = await employeeService.GetByFilterAsync(empl => empl.FirstName == "Александр",
                c => c.OrderBy(cl => cl.Age), 1, 1);

            //Assert
            Assert.Single(employeeByName);
            Assert.Empty(result);
        }
    }
}

