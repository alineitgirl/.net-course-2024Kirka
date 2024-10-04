using System;

namespace BankSystem.App.Exceptions
{
    public class NoInfoAboutPassportNumberException : Exception
    {
        public NoInfoAboutPassportNumberException(string message) : base(message)
        {
            
        }
        
    }
}