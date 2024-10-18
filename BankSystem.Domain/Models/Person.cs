using System;

namespace BankSystem.Domain.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string? Adress { get; set; } 
        public string? Passport { get; set; }
        public string? PhoneNumber { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public override string ToString() =>  $"Имя: {FirstName}\n" +
                                              $"Фамилия: {LastName}\n" +
                                              $"Дата рождения: {DateOfBirth}\n" +
                                              $"Номер телефона: {PhoneNumber}\n" +
                                              $"Адрес проживания: {Adress}\n";

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Person))
                return false;
            var person = (Person) obj;
            return Id == person.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}