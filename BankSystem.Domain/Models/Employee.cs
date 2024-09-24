using System;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        private Guid Id { get; set; }
        private string Position { get; set; }= String.Empty;
        private string Department { get; set; }= String.Empty;
        public double Salary { get; set; } = 0;
        public string Contract { get; set; }= "Контракт сотрудника по умолчанию";
        private string Skills { get; set; } = String.Empty;


        public Employee (string firstName, string lastName, DateTime dateOfBirth, string address, string passport,string phoneNumber,
            string position, string department, double salary, string skills): base(firstName, lastName, dateOfBirth,
            address, passport, phoneNumber) {
            this.Id = Guid.NewGuid();
            this.Position = position;
            this.Department = department;
            this.Salary = salary;
            this.Skills = skills;
        }
        
        public override string ToString()
        {
            return  base.ToString() + $"Отдел: {Department}\n" +
                    $"Заработная плата: {Salary}\n" +
                    $"Контракт: {Contract}\n" +
                    $"Навыки: {Skills}\n\n\n";
        }
    }
}