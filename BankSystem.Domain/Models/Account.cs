using System;

namespace BankSystem.Domain.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string? CurrencyName { get; set; }
        public double Amount { get; set; }
        
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is Account))
            {
                return false;
            }

            var account = (Account) obj;
            return Id == account.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}