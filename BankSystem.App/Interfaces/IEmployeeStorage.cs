using System;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeStorage : IStorage<Employee>
    {
        public void Update(Guid id, string firstName, string lastName, DateTime birthDate,
            string phoneNumber, string passport, string address, int age, string position,
            double salary, string department);
    }
}