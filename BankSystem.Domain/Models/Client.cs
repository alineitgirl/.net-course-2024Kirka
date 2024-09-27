using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace BankSystem.Domain.Models
{
    public class Client : Person
    {

        public Guid Id { get; set; }
        public double AccountBalance = 0;
        public string Preferences = String.Empty;
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

        public Client(Client other)
        {
            FirstName = other.FirstName;
            LastName = other.LastName;
            DateOfBirth = other.DateOfBirth;
            Adress = other.Adress;
            Passport = other.Passport;
            PhoneNumber = other.PhoneNumber;
            AccountBalance = other.AccountBalance;
            Preferences = other.Preferences;
            Age = other.Age;
        }
        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Client))
                return false;
            var client = (Client) obj;
            return this.Id == client.Id && this.PhoneNumber == client.PhoneNumber && this.Passport == client.Passport;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + PhoneNumber.GetHashCode() + Passport.GetHashCode();
        }
    }
}