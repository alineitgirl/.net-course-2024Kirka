using System;
namespace BankSystem.App.Exceptions
{
    public class AgeOutOfRangeException : Exception
    {
        public AgeOutOfRangeException(string message) : base(message)
        {
            
        }
    }
}