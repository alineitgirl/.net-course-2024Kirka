using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Data.DbContext;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Newtonsoft.Json;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BankSystem.Data.Tests;

public class TasksTests
{
    private BlockingCollection<(Guid, Account?, double, CancellationToken)> collectionOfRequests;
    private List<Task> consumerTasks;
    private CountdownEvent countdownEvent;
    private object lockObject = new();
    private readonly ITestOutputHelper output;
    
    public TasksTests(ITestOutputHelper testOutputHelper)
    {
        output = testOutputHelper;
    }

    
    [Fact]
    public async void AddMoneyToAccountTest()
    {
        // Arrange
        var clientStorage = new ClientStorage(new BankSystemDbContext());
        var rateUpdater = new RateUpdater(clientStorage, 0.03);
        
        var client = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Baker",
            DateOfBirth = DateTime.Today.AddYears(-20).ToUniversalTime(),
            Passport = "ID123456",
            PhoneNumber = "123456777",
            Adress = "st. Sherlock Holmes",
            Age = 20,
            CreatedOn = DateTime.Now.AddMonths(-1).ToUniversalTime(), 
            Accounts = new List<Account>
            {
                new Account { Id = Guid.NewGuid(), Amount = 1000, CurrencyName = "USD", CreatedOn = DateTime.Now.AddMonths(-1).ToUniversalTime() }
            }
        };
        
        await clientStorage.AddAsync(client);
        
        var clientBeforeUpdate = await clientStorage.GetClientWithAccountsAsync(client.Id);
        Assert.NotNull(clientBeforeUpdate);
        var accountBeforeUpdate = clientBeforeUpdate.Accounts.FirstOrDefault();
        Assert.NotNull(accountBeforeUpdate);

        // Act
        await Task.Run(() => rateUpdater.UpdateRateAsync());
        
        // Assert
        var clientAfterUpdate = await clientStorage.GetClientWithAccountsAsync(client.Id);
        var accountAfterUpdate = clientAfterUpdate.Accounts.FirstOrDefault();

        var expectedAmount = accountBeforeUpdate.Amount * 1.03;
        Assert.Equal(expectedAmount, accountAfterUpdate.Amount);
    }

    [Fact]
    public async Task TestSystem()
    {
        collectionOfRequests = new BlockingCollection<(Guid, Account, double, CancellationToken)>(boundedCapacity: 10); 
        consumerTasks = new List<Task>();
        int consumerCount = 3;
        countdownEvent = new CountdownEvent(consumerCount);

        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<EmployeeDto, Employee>()));
        var clientService = new ClientService(new ClientStorage(new BankSystemDbContext()), mapper);
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
       
        for (int i = 0; i < consumerCount; i++)
        {
            consumerTasks.Add(Task.Run(() => ProcessWithdrawals(clientService)));
        }
        
        var clients = await clientService.GetByFilterAsync( cl => true, cl => cl.OrderBy(
            c => c.Id), 1, 10, token);
        foreach (var client in clients)
        {
            var clWithAccounts = await clientService.GetClientWithAccountAsync(client.Id, token);
            var accounts = clWithAccounts.Accounts.FirstOrDefault(a => a.Id == client.Id);
            collectionOfRequests.Add((client.Id, accounts, 100, token));
        }

        collectionOfRequests.CompleteAdding();

        await Task.WhenAll(consumerTasks);
        
        output.WriteLine("Все запросы обработаны.");

        
        foreach (var client in clients)
        {
            var clientWithAccount = await clientService.GetClientWithAccountAsync(client.Id, token);
            var accounts = clientWithAccount.Accounts.FirstOrDefault(a => a.CurrencyName == "USD");
            Assert.Equal(900, accounts.Amount); 
        }
    }
    
    private async Task ProcessWithdrawals(ClientService clientService)
    {
        foreach (var (clientId, account, amount, token) in collectionOfRequests.GetConsumingEnumerable())
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            await clientService.WriteOffMoney(clientId, account, amount, token);
        }
        countdownEvent.Signal();
    }
}