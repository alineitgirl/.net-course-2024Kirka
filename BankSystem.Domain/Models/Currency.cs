using System;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string CurrencyCode;
        public string CurrencyName;
        public string CurrencySymbol;
        
        public override string ToString() => $"Код валюты: {CurrencyCode}\n" +
                                           $"Название валюты: {CurrencyName}\n" +
                                           $"Символ валюты: {CurrencySymbol}\n";
    }
}