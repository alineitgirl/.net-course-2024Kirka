using System;

namespace BankSystem.Domain.Models
{
    public class Account
    {
        private string _currency = String.Empty;
        public double Amount = 0;

        public Account(string currency, double amount)
        {
            _currency = currency;
            Amount = amount;
        }
    }
}