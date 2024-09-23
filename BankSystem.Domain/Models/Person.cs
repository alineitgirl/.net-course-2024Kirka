﻿using System;

namespace BankSystem.Domain.Models
{
    public class Person
    {
        private string FirstName { get; set; } = String.Empty;
        private string LastName { get; set; } = String.Empty;
        private DateTime DateOfBirth { get; set; } = DateTime.Now;
        private string Adress { get; set; } = String.Empty;
        private string Passport { get; set; } = String.Empty;
        private string PhoneNumber { get; set; } = string.Empty;

        public Person(string firstName, string lastName, DateTime dateOfBirth, string adress, string passport,
            string phoneNumber)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Adress = adress;
            this.Passport = passport;
            this.PhoneNumber = phoneNumber;
        }
        
        public override string ToString() =>  $"Имя: {FirstName}\n" +
                                            $"Фамилия: {LastName}\n" +
                                            $"Дата рождения: {DateOfBirth}\n" +
                                            $"Номер телефона: {PhoneNumber}\n" +
                                            $"Адрес проживания: {Adress}\n";
    }
}