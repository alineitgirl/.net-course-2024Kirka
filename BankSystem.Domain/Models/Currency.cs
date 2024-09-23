using System;
using System.Runtime.InteropServices;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string CurrencyCode;
        public string CurrencyName;
        public string CurrencySymbol;

        public Currency(string currencyCode ="RUP", string currencyName = "Рубли ПМР", string currencySymbol = "P")
        {
            this.CurrencyCode = currencyCode;
            this.CurrencyName = currencyName;
            this.CurrencySymbol = currencySymbol;
        }

        public override string ToString() => $"Код валюты: {CurrencyCode}\n" +
                                           $"Название валюты: {CurrencyName}\n" +
                                           $"Символ валюты: {CurrencySymbol}\n";
    }
}