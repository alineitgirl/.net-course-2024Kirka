using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace BankSystem.Domain.Models
{
    public class Client : Person
    {

        public Guid Id { get; set; }
        public double AccountBalance = 0;
        public int Age { get; set; } = 0;

        public override string ToString()
        {
            return base.ToString() + $"\nОстаток на счету: {AccountBalance}\n" +
                   $"Возраст: {Age}\n\n\n";
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