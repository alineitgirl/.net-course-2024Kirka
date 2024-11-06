using Xunit.Abstractions;
using Xunit.Sdk;

namespace BankSystem.App.Services.Tests;

public class TestCurrencyService
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TestCurrencyService(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public async void EcxhangeCurrency_PositivTest()
    {
        //Arrange
        var currencyService = new CurrencyService();
        var baseCurrency = "USD";
        var targetCurrency = "EUR";
        var amount = 20000;
        
        //Act
         var response = await currencyService.Exchange(baseCurrency, targetCurrency, amount);
         
        //Assert
        _testOutputHelper.WriteLine(response.ErrorCode > 0
            ? response.ErrorMessage
            : $"Result: {amount} {baseCurrency} = {response.Amount} {targetCurrency}");
    }
}