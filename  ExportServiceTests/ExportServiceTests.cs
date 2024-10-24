using BankSystem.App.Services;
using BankSystem.Data.DbContext;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using ExportTool;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace ExportServiceTests;

public class ExportTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public ExportTest(ITestOutputHelper testOutputHelper)
    { 
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void ExportDataToCsvFile_Positiv_Test()
    {
        //Arrange
        var clientStorage = new ClientStorage(new BankSystemDbContext()); 
        var clientService = new ClientService(clientStorage);
        
        //Act
        var clients = clientService.GetByFilter(client => true, cl => cl.Id, cl => cl.Id, 
            1, 50).ToList();
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.csv";
        var exportService = new ExportService(); 
        exportService.ExportDataToCsvFile(clients, Path.Combine(pathToDirectory, fileName));
       
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileName)}");
    }
    
    [Fact]
    public void ImportClientsFromCsvFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "import_clients.csv";
        var exportService = new ExportService();
        
        
        //Act
        var newClients = exportService.ImportClientFromCsvFile<Client>(Path.Combine(pathToDirectory, fileName)).ToList();
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        foreach (var client in newClients)
        {
            client.DateOfBirth = client.DateOfBirth.ToUniversalTime();
            client.CreatedOn = client.CreatedOn.ToUniversalTime();
            clientStorage.Add(client);
        }
        
        //Assert
        Assert.Equal(8, File.ReadAllLines(Path.Combine(pathToDirectory, fileName)).Length -1);
    }
    
    [Fact]
    public void SerializeClientsToJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.json";
        var exportService = new ExportService();
        
        //Act
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var clients = clientStorage.GetByFilter(client => true, cl => cl.Id, cl => cl.Id,
            1, 50).ToList();
        exportService.ExportDataToJsonFile(clients, Path.Combine(pathToDirectory, fileName));
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileName)}");
    }

    [Fact]
    public void DeserializeClientsFromJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.json";
        var exportService = new ExportService();
        
        //Act
        var selectedClients = exportService.ImportDataFromJsonFile<Client>
            (Path.Combine(pathToDirectory, fileName)).ToList();
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var clients = clientStorage.GetByFilter(client => true, cl => cl.Id, cl => cl.Id,
            1, 50).ToList();
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        Assert.Equal(selectedClients, clients);
    }

    [Fact]
    public void SerializeEmployeesToJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_employees.json";
        var exportService = new ExportService();
        
        //Act
        var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
        var employees = employeeStorage.GetByFilter(client => true, cl => cl.Id, cl => cl.Id,
            1, 50).ToList();
        exportService.ExportDataToJsonFile(employees, Path.Combine(pathToDirectory, fileName));
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileName)}");
    }

    [Fact]
    public void DeserializeEmployeesFromJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_employees.json";
        var exportService = new ExportService();
        
        //Act
        var selectedEmployees = exportService.ImportDataFromJsonFile<Employee>
            (Path.Combine(pathToDirectory, fileName)).ToList();
        var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
        var employees = employeeStorage.GetByFilter(client => true, cl => cl.Id, cl => cl.Id,
            1, 50).ToList();
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        Assert.Equal(selectedEmployees, employees);
    }

    [Fact]
    public void ExportAOneClientOrOneEmployee_Positiv_Test()
    { //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileNameForEmployee = "export_one_employee.json";
        var fileNameForClient = "export_one_client.json";
        var exportService = new ExportService();
        
        //Act
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var clients = clientStorage.GetByFilter(client => true, cl => cl.Id, cl => cl.Id,
            1, 1).ToList();
        var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
        var employees = employeeStorage.GetByFilter(employee => true, cl => cl.Id, cl => cl.Id,
            1,1).ToList();
        exportService.ExportDataToJsonFile(clients, Path.Combine(pathToDirectory, fileNameForClient));
        exportService.ExportDataToJsonFile(employees, Path.Combine(pathToDirectory, fileNameForEmployee));
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileNameForEmployee)));
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileNameForClient)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileNameForEmployee)}");
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileNameForClient)}"); }
}