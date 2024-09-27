using System;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public Guid Id { get; set; }
        public string Position { get; set; }= String.Empty;
        public string Department { get; set; }= String.Empty;
        public double Salary { get; set; } = 0;
        public string Contract { get; set; }= "Контракт сотрудника по умолчанию";
        public string Skills { get; set; } = String.Empty;


        public Employee (string firstName, string lastName, DateTime dateOfBirth, string address, string passport,string phoneNumber,
            string position, string department, double salary, string skills): base(firstName, lastName, dateOfBirth,
            address, passport, phoneNumber) {
            this.Id = Guid.NewGuid();
            this.Position = position;
            this.Department = department;
            this.Salary = salary;
            this.Skills = skills;
        }

        public Employee(Employee other)
        {
            FirstName = other.FirstName;
            LastName = other.LastName;
            DateOfBirth = other.DateOfBirth;
            Adress = other.Adress;
            Passport = other.Passport;
            PhoneNumber = other.PhoneNumber;
            Position = other.Position;
            Department = other.Department;
            Salary = other.Salary;
            Skills = other.Skills;
        }
        
        public override string ToString()
        {
            return  base.ToString() + $"Отдел: {Department}\n" +
                    $"Заработная плата: {Salary}\n" +
                    $"Контракт: {Contract}\n" +
                    $"Навыки: {Skills}\n\n\n";
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Employee))
                return false;
            var employee = (Employee) obj;
            return this.Id == employee.Id && this.PhoneNumber == employee.PhoneNumber && this.Passport == employee.Passport;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + PhoneNumber.GetHashCode() + Passport.GetHashCode();
        }
    }
}