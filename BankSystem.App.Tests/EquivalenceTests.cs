using System.Collections.Generic;
using System.Linq;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.App.Tests
{
    public class EquivalenceTests
    {
        [Fact]
        public void GetHashCodeNecessityPositivTest()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
        
            //Act
            var clientsDictionary = testDataGenerator.GenerateClientsDictionary(10);
            var searchedClient = clientsDictionary.FirstOrDefault(
                cl => cl.Key.PhoneNumber == "70000").Key;
            var newClient = new Client
            {
                Id = searchedClient.Id,
                FirstName =  searchedClient.FirstName,
                LastName =  searchedClient.LastName,
                AccountBalance = searchedClient.AccountBalance,
                Adress = searchedClient.Adress,
                Age = searchedClient.Age,
                DateOfBirth = searchedClient.DateOfBirth,
                Passport = searchedClient.Passport,
                PhoneNumber = searchedClient.PhoneNumber
            };

            //Assert
            Assert.True(clientsDictionary.TryGetValue(newClient, out List<Account> accounts));
        }
    
        [Fact]
        public void CheckIfClientHasManyAccounts()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
        
            //Act
            var clientsDictionary = testDataGenerator.GenerateClientsDictionary(10);
            var richClient = clientsDictionary.FirstOrDefault(
                cl => cl.Key.PhoneNumber == "70007").Key;

            //Assert
            Assert.Equal(2, clientsDictionary[richClient].Count);
        }
    
        [Fact]
        public void EmployeesWithSameData_AreEqual()
        {
            //Arrange
            TestDataGenerator testDataGenerator = new TestDataGenerator();
        
            //Act
            var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
            var searchedEmployee = listOfEmployees.FirstOrDefault(emp => emp.PhoneNumber == "70035");
            var newEmployee = new Employee
            {
                Id = searchedEmployee.Id,
                FirstName = searchedEmployee.FirstName,
                LastName = searchedEmployee.LastName,
                Adress = searchedEmployee.Adress,
                Contract = searchedEmployee.Contract,
                DateOfBirth = searchedEmployee.DateOfBirth,
                Department = searchedEmployee.Department,
                Passport = searchedEmployee.Passport,
                PhoneNumber = searchedEmployee.PhoneNumber
            };
        
            //Assert
            Assert.True(searchedEmployee.Equals(newEmployee));
        }
    }
}

