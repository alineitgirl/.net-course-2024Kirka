using System;
using System.Data;
using System.Text.RegularExpressions;
using BankSystem.App.Dto;
using FluentValidation;

namespace BankSystem.App.Validators
{
    public class ClientDtoValidator : AbstractValidator<ClientDto>
    {
        public ClientDtoValidator()
        {
            RuleFor(o => o.FullName)
                .Length(1, 100)
                .NotEmpty()
                .NotNull()
                .WithMessage("Имя клиента обязтательно.");
            
            RuleFor(o => o.PhoneNumber)
                .MaximumLength(20)
                .NotEmpty()
                .NotNull()
                .WithMessage("Номер телефона для клиента обязателен.");
            
            RuleFor(o => o.DateOfBirth)
                .NotEmpty()
                .WithMessage("Не указана дата рождения.");
            
            RuleFor(o => o.Address)
                .NotEmpty()
                .Length(1, 100)
                .WithMessage("Не указан адрес клиента");

            RuleFor(o => o.Age).GreaterThan(18);

            RuleFor(p => p.Passport)
                .NotEmpty()
                .NotNull()
                .WithMessage("Паспортный номер обязателен.")
                .Length(1, 20);
        }
    }
}