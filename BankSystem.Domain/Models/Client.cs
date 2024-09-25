using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace BankSystem.Domain.Models
{
    public class Client : Person
    {

        private Guid Id { get; }
        private double AccountBalance = 0;
        private string Preferences = String.Empty;
        public int Age { get; set; } = 0;
        
        public Client (string firstName, string lastName, DateTime dateOfBirth, string address, string passport, string phoneNumber,
            double accountBalance, string preferences, int age): base(firstName, lastName, dateOfBirth,
            address, passport, phoneNumber) {
            this.Id = Guid.NewGuid();
            this.AccountBalance = accountBalance;
            this.Preferences = preferences;
            this.Age = age;
        }
        
        public override string ToString()
        {
            return base.ToString() + $"\nОстаток на счету: {AccountBalance}\n" +
                   $"Предпочтения: {Preferences}\n" +
                   $"Возраст: {Age}\n";
        }
        
    }
}