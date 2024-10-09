using System;

namespace BankSystem.Domain.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string Adress { get; set; } 
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        
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
            return this.PhoneNumber == person.PhoneNumber && this.FirstName == person.FirstName
                                                          && this.LastName == person.LastName && this.Passport == person.Passport;
        }

        public override int GetHashCode()
        {
            return PhoneNumber.GetHashCode() + FirstName.GetHashCode() + LastName.GetHashCode() + Passport.GetHashCode();
        }
    }
}