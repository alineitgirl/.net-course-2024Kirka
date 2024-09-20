using System;
using System.Runtime.InteropServices;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string CurrencyCode;
        public string CurrencyName;
        public string CurrencySymbol;
        public decimal ExchangeRate;

        public Currency(string currencyCode ="RUP", string currencyName = "Рубли ПМР", string currencySymbol = "P", 
            decimal exchangeRate = Decimal.One)
        {
            this.CurrencyCode = currencyCode;
            this.CurrencyName = currencyName;
            this.CurrencySymbol = currencySymbol;
            this.ExchangeRate = exchangeRate;
        }

        public void Print() => Console.WriteLine($"Код валюты: {CurrencyCode}\n" +
                                                 $"Название валюты: {CurrencyName}\n" +
                                                 $"Символ валюты: {CurrencySymbol}\n" +
                                                 $"Курс обмена: {ExchangeRate}\n\n\n");
    }
}