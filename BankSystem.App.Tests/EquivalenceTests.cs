using System.Collections.Generic;
using System.Linq;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Xunit;
namespace BankSystem.App.Tests;

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
        var newClient = new Client(searchedClient) {Id = searchedClient.Id};

        //Assert
        Assert.False(clientsDictionary.TryGetValue(newClient, out List<Account> accounts));
    }

    [Fact]
    public void GetHashCodeOverrideTest()
    {
        //Arrange
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        
        //Act
        var clientsDictionary = testDataGenerator.GenerateClientsDictionary(10);
        var searchedClient = clientsDictionary.FirstOrDefault(
            cl => cl.Key.PhoneNumber == "70000").Key;
        var newClient = new Client(searchedClient) {Id = searchedClient.Id};

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
    public void CompareEmployeesWithoutOverrideGetHashCode()
    {
        //Arrange
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        
        //Act
        var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
        var searchedEmployee = listOfEmployees.FirstOrDefault(emp => emp.PhoneNumber == "70035");
        var newEmployee = new Employee(searchedEmployee) {Id = searchedEmployee.Id};

        //Assert
        Assert.False(searchedEmployee.Equals(newEmployee));
    }

    [Fact]
    public void CompareEmployeesWithOverrideGetHashCode()
    {
        //Arrange
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        
        //Act
        var listOfEmployees = testDataGenerator.GenerateListOfEmployees(10);
        var searchedEmployee = listOfEmployees.FirstOrDefault(emp => emp.PhoneNumber == "70035");
        var newEmployee = new Employee(searchedEmployee) {Id = searchedEmployee.Id};
        
        //Assert
        Assert.True(searchedEmployee.Equals(newEmployee));
    }
}