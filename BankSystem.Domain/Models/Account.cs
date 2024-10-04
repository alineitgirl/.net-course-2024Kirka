using System;

namespace BankSystem.Domain.Models
{
    public class Account
    {
        public string Currency { get; set; }
        public double Amount = 0;

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Account))
            {
                return false;
            }

            var account = (Account) obj;
            return Math.Abs(Amount - account.Amount) < 0.002 && Currency == account.Currency;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode() + Currency.GetHashCode();
        }
    }
}