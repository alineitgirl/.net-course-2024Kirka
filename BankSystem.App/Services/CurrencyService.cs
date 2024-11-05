using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BankSystem.App.Services
{
    public class CurrencyResponse
    {
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public double Amount { get; set; }
    }
    public class CurrencyService
    {
        CurrencyResponse? currencyResponse;
        public async Task<CurrencyResponse?> Exchange(string baseCurrency, string targetCurrency, double amount)
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync(
                        $"https://www.amdoren.com/api/currency.php?api_key=DLRK5mWMn2j35DPavSFQbcrSrULnNw&from={baseCurrency}&to={targetCurrency}&amount={amount}");
                response.EnsureSuccessStatusCode();
                var message = await response.Content.ReadAsStringAsync();
                currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(message);
                
            }
            return currencyResponse;
        }
    }
}