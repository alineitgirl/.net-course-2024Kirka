using System;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public Guid Id { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public double Salary { get; set; } = 0;
        public string Contract { get; set; }= "Контракт сотрудника по умолчанию";
        public int Age { get; set; }


        public override string ToString()
        {
            return  base.ToString() + $"Отдел: {Department}\n" +
                    $"Заработная плата: {Salary}\n" +
                    $"Контракт: {Contract}\n\n\n";
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Employee))
                return false;
            var employee = (Employee) obj;
            return this.PhoneNumber == employee.PhoneNumber && this.FirstName == employee.FirstName
                && this.LastName == employee.LastName && this.Passport == employee.Passport;
        }

        public override int GetHashCode()
        {
            return PhoneNumber.GetHashCode() + FirstName.GetHashCode() + LastName.GetHashCode() + 
                Passport.GetHashCode();
        }
    }
}