using System;
using BankSystem.App.Dto;
using FluentValidation;
namespace BankSystem.App.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(empl => empl.FullName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Имя сотрудника обязательно.");
            
            RuleFor(o => o.PhoneNumber)
                .MaximumLength(20)
                .NotEmpty()
                .NotNull()
                .Matches(@"^0-\d{3}-\d{3}-\d{2}$")
                .WithMessage("Номер телефона для клиента обязателен.");
            
            RuleFor(o => o.DateOfBirth)
                .NotEmpty()
                .WithMessage("Не указана дата рождения.");
            
            RuleFor(o => o.Adress)
                .NotEmpty()
                .Length(1, 100)
                .WithMessage("Не указан адрес клиента");

            RuleFor(o => o.Position)
                .NotEmpty()
                .Length(1, 100)
                .NotNull()
                .WithMessage("Должность сотруждника обязательна.");

            RuleFor(o => o.Department)
                .NotEmpty()
                .Length(1, 100)
                .NotNull()
                .WithMessage("Отдел сотрудника обязателен.");
        }
    }
}