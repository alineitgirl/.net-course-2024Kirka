using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace BankSystem.Domain.Models
{
    public class Client : Person
    {

        private Guid Id { get; }
        private string CustomerStatus { get; set; } = "active";
        private decimal AccountBalance = Decimal.Zero;
        private List<DateTime> PurchaseHistory = null;
        private string Preferences = String.Empty;
        
        public Client() {
    
        }
    
        public Client (string firstName, string lastName, DateTime dateOfBirth, string address, string passport, int phoneNumber,
            string customerStatus, decimal accountBalance, DateTime purchase, string preferences): base(firstName, lastName, dateOfBirth,
            address, passport, phoneNumber) {
            this.Id = new Guid();
            this.CustomerStatus = customerStatus;
            this.AccountBalance = accountBalance;
            PurchaseHistory.Add(purchase);
            this.Preferences = preferences;
        }
    }
}