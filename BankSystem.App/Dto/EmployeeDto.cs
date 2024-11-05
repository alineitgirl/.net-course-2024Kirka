using System;

namespace BankSystem.App.Dto
{
    public class EmployeeDto
    {
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Adress { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string Passport { get; set; }
    }
}