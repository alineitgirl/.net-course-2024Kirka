using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankService
    {

        public int? CountOwnersSalary(decimal profit, decimal expenses, int numberOfEmployees)
        {
            if (numberOfEmployees != 0)
            {
                return (int) (profit - expenses) / numberOfEmployees;
            }
            else return -1;
        }
    }
}