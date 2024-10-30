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
    public async Task ExportDataToCsvFile_Positiv_Test()
    {
        //Arrange
        var clientStorage = new ClientStorage(new BankSystemDbContext()); 
        var clientService = new ClientService(clientStorage);
        
        //Act
        var clients = await clientService.GetByFilterAsync(client => true, cl => 
                cl.OrderBy(c => c.Id), 1, 50);
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.csv";
        var exportService = new ExportService(); 
        exportService.ExportDataToCsvFile(clients, Path.Combine(pathToDirectory, fileName));
       
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileName)}");
    }
    
    [Fact]
    public async Task ImportClientsFromCsvFile_Positiv_Test()
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
            await clientStorage.AddAsync(client);
        }
        
        //Assert
        Assert.Equal(8, File.ReadAllLines(Path.Combine(pathToDirectory, fileName)).Length -1);
    }
    
    [Fact]
    public async Task SerializeClientsToJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.json";
        var exportService = new ExportService();
        
        //Act
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var clients = await clientStorage.GetByFilterAsync(client => true, cl => 
                cl.OrderBy(c => c.Id),  1, 50);
        exportService.ExportDataToJsonFile(clients, Path.Combine(pathToDirectory, fileName));
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileName)}");
    }

    [Fact]
    public  async Task DeserializeClientsFromJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.json";
        var exportService = new ExportService();
        
        //Act
        var selectedClients = exportService.ImportDataFromJsonFile<Client>
            (Path.Combine(pathToDirectory, fileName)).ToList();
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var clients = await clientStorage.GetByFilterAsync(client => true, cl => 
                cl.OrderBy(c => c.Id), 1, 50);
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        Assert.Equal(selectedClients, clients);
    }

    [Fact]
    public async Task SerializeEmployeesToJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_employees.json";
        var exportService = new ExportService();
        
        //Act
        var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
        var employees = await employeeStorage.GetByFilterAsync(client => true, cl 
                => cl.OrderBy(c => c.Id), 1, 50);
        exportService.ExportDataToJsonFile(employees, Path.Combine(pathToDirectory, fileName));
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileName)}");
    }

    [Fact]
    public async Task DeserializeEmployeesFromJsonFile_Positiv_Test()
    {
        //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_employees.json";
        var exportService = new ExportService();
        
        //Act
        var selectedEmployees = exportService.ImportDataFromJsonFile<Employee>
            (Path.Combine(pathToDirectory, fileName)).ToList();
        var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
        var employees =  await employeeStorage.GetByFilterAsync(client => true, 
            cl => cl.OrderBy(c => c.Id), 1, 50);
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileName)));
        Assert.Equal(selectedEmployees, employees);
    }

    [Fact]
    public async Task ExportAOneClientOrOneEmployee_Positiv_Test()
    { //Arrange
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileNameForEmployee = "export_one_employee.json";
        var fileNameForClient = "export_one_client.json";
        var exportService = new ExportService();
        
        //Act
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var clients = await clientStorage.GetByFilterAsync(client => true, cl => cl.OrderBy(
                c => c.Id), 1, 1);
        var employeeStorage = new EmployeeStorage(new BankSystemDbContext());
        var employees =  await employeeStorage.GetByFilterAsync(employee => true, cl
                => cl.OrderBy(c => c.Id), 1,1);
        exportService.ExportDataToJsonFile(clients, Path.Combine(pathToDirectory, fileNameForClient));
        exportService.ExportDataToJsonFile(employees, Path.Combine(pathToDirectory, fileNameForEmployee));
        
        //Assert
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileNameForEmployee)));
        Assert.True(File.Exists(Path.Combine(pathToDirectory, fileNameForClient)));
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileNameForEmployee)}");
        _testOutputHelper.WriteLine($"File created on {Path.Combine(pathToDirectory, fileNameForClient)}"); }
}