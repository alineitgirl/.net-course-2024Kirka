using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;


namespace BankSystem.Domain.Models
{
    public class Client : Person
    {
        public int Age { get; set; } 
        public ICollection<Account>? Accounts { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Возраст: {Age}\n\n\n";
        }
        
        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Client))
                return false;
            var client = (Client) obj;
            return Id == client.Id;
        }

        public override int GetHashCode()
        {
            return  Id.GetHashCode();
        }
    }
}