using System;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public string Position { get; set; }
        public string Department { get; set; }
        public double Salary { get; set; }
        public string Contract { get; set; }
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
            return Id == employee.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}