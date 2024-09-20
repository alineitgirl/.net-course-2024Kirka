using System;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        private Guid Id { get; set; }
        private string Position { get; set; }= String.Empty;
        private string Department { get; set; }= String.Empty;
        private double Salary { get; set; } = 0;
        public string Contract { get; set; }= "Контракт сотрудника по умолчанию";
        private string Skills { get; set; } = String.Empty;

        public Employee()
        {
        }

        public Employee (string firstName, string lastName, DateTime dateOfBirth, string address, string passport, int phoneNumber,
            string position, string department, double salary, string skills): base(firstName, lastName, dateOfBirth,
            address, passport, phoneNumber) {
            this.Id = new Guid();
            this.Position = position;
            this.Department = department;
            this.Salary = salary;
            this.Skills = skills;
        }
        
        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Должность: {Position}\n" +
                              $"Отдел: {Department}\n" +
                              $"Контракт: {Contract}\n" +
                              $"Навыки: {Skills}\n\n\n");
        }
    }
}