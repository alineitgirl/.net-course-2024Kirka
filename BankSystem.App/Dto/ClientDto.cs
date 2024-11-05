using System;

namespace BankSystem.App.Dto
{
    public class ClientDto 
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int Age { get; set; }
        public string Passport { get; set; }
    }
}