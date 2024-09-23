using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace BankSystem.Domain.Models
{
    public class Client : Person
    {

        private Guid Id { get; }
        private decimal AccountBalance = Decimal.Zero;
        private string Preferences = String.Empty;
        
        public Client (string firstName, string lastName, DateTime dateOfBirth, string address, string passport, string phoneNumber,
            decimal accountBalance, string preferences): base(firstName, lastName, dateOfBirth,
            address, passport, phoneNumber) {
            this.Id = new Guid();
            this.AccountBalance = accountBalance;
            this.Preferences = preferences;
        }
    }
}