using System.Collections.Concurrent;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using ExportTool;
using Xunit.Abstractions;

namespace ThreadAndTaskTests;

public class ThreadTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private BlockingCollection<Client> _clientCollection;
    private readonly int _maxFileSize = 1024 * 10;
    private CountdownEvent _countdownEvent;
    private int _fileIndex = 0;
    private readonly string _pathToDirectory = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).
        Parent.Parent.Parent.FullName);
    private readonly object _lockObject = new();


    public ThreadTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestImportAndExport()
    {
        _clientCollection = new BlockingCollection<Client>(boundedCapacity: 20);
        var consumerCount = 5;
        _countdownEvent = new CountdownEvent(consumerCount);


        Thread producerThread = new Thread(Producer);
        producerThread.Start();


        for (int i = 0; i < consumerCount; i++)
        {
            Thread consumerThread = new Thread(() => Consumer(_countdownEvent));
            consumerThread.Start();
        }


        _countdownEvent.Wait();
        _testOutputHelper.WriteLine("Все клиенты обработаны и сериализованы.");

        var exportService = new ExportService();
        for (var i = 1; i <= _fileIndex; i++)
        {
            _testOutputHelper.WriteLine($"\nКлиенты из файла ClientData_{i}.json");
            var clientsFromFile = exportService.ImportDataFromJsonFile<Client>
                (Path.Combine(_pathToDirectory, $"ClientData_{i}.json")).ToList();
            if (clientsFromFile is List<Client> clients)
            {
                foreach (var cl in clients)
                {
                    _testOutputHelper.WriteLine(cl.ToString());
                }
            }
            else
            {
                _testOutputHelper.WriteLine("Что-то пошло не так. Ошибка чтения из файла!");
            }
        }
    }

    private void Producer()
    {
        var testDataGenerator = new TestDataGenerator();
        var newClients = testDataGenerator.GenerateListOfClients(100);

        foreach (var client in newClients)
        {
            _clientCollection.Add(client);
            Thread.Sleep(100);
        }
        
        _clientCollection.CompleteAdding();
    }

    private void Consumer(CountdownEvent countdownEvent)
    {
        var clientsToWrite = new List<Client>();
        var currentFile = GetNextFileName();
        var exportService = new ExportService();

        foreach (var client in _clientCollection.GetConsumingEnumerable())
        {
            clientsToWrite.Add(client);

            lock (_lockObject)
            {
                if (File.Exists(currentFile) && new FileInfo(currentFile).Length < _maxFileSize)
                {
                    exportService.ExportDataToJsonFile(clientsToWrite, Path.Combine(_pathToDirectory, currentFile));
                    clientsToWrite.Clear();
                    currentFile = GetNextFileName();
                }
            }
        }
        
        if (clientsToWrite.Count > 0)
        {
            lock (_lockObject)
            {
                exportService.ExportDataToJsonFile(clientsToWrite, Path.Combine(_pathToDirectory, currentFile));
            }
        }
        _countdownEvent.Signal();
    }

    private string GetNextFileName()
    {
        return $"ClientData_{Interlocked.Increment(ref _fileIndex)}.json";
    }
    
    [Fact]
    public void AddMoneyToAccount()
    {
        var testAccount = new Account { Amount = 0, CurrencyName = "USD" };
        var count = 10;
        var amountToAdd = 100;
        _countdownEvent  = new CountdownEvent(2);

        ThreadPool.QueueUserWorkItem(_ =>
        {
            var c = count;
            while (c-- > 0)
            {
                lock (_lockObject)
                {
                    testAccount.Amount += amountToAdd;
                }
            }
            _countdownEvent.Signal();
        });

        ThreadPool.QueueUserWorkItem(_ =>
        {
            var c = count;
            while (c-- > 0)
            {
                lock (_lockObject)
                {
                    
                    testAccount.Amount += amountToAdd;
                }
            }
            _countdownEvent.Signal();
        });
        
        _countdownEvent.Wait();
        _testOutputHelper.WriteLine("Все потоки завершены.");
         Assert.Equal(2 * count * amountToAdd, testAccount.Amount);
    }
}