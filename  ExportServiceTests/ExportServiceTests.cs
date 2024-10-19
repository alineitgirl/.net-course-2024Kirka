using BankSystem.App.Services;
using BankSystem.Data.DbContext;
using BankSystem.Data.Storages;
using ExportTool;
using Xunit.Abstractions;
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
    public void WriteDataToFile_Positiv_Test()
    {
        //Arrange
        var clientStorage = new ClientStorage(new BankSystemDbContext()); 
        var clientService = new ClientService(clientStorage);
        
        //Act
        var clients = clientService.GetByFilter(client => true, cl => cl.Id, cl => cl.Id, 
            1, 10).ToList();
        var pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName);
        var fileName = "export_clients.csv";
        var exportService = new ExportService(pathToDirectory, fileName); 
        exportService.ExportClientToCsvFile(clients);
       
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
        var exportService = new ExportService(pathToDirectory, fileName);
        
        
        //Act
        exportService.ImportClientFromFile(Path.Combine(pathToDirectory, fileName));
        
        //Assert
        Assert.Equal(8, File.ReadAllLines(Path.Combine(pathToDirectory, fileName)).Length -1);
    }
    
}