using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using AutoMapper;
using BankSystem.App.Dto;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private IEmployeeStorage _employeeStorage;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeStorage employeeStorage, IMapper mapper)
        {
            _employeeStorage = employeeStorage;
            _mapper = mapper;
        }
        
        public async Task AddEmployeeAsync(EmployeeDto employeeDto, CancellationToken cancellationToken = default)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            if (employee.Age < 18)
            {
                throw new AgeOutOfRangeException("Сотрудник не может быть моложе 18 лет!");
            }

            if (string.IsNullOrEmpty(employee.Passport))
            {
                throw new NoInfoAboutPassportNumberException("Не указаны паспортные данные у сотрудника!");
            } 
            await _employeeStorage.AddAsync(employee, cancellationToken);
        }

        public async Task UpdateEmployeeAsync(Guid id, EmployeeDto employeeDto, CancellationToken cancellationToken = default)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _employeeStorage.UpdateAsync(id, employee, cancellationToken);
        }
        
        public async Task<List<Employee>> GetByFilterAsync(
            Expression<Func<Employee, bool>> filter = null, Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = null, 
            int pageNumber = 1, int pageSize = 1, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            var employees = await _employeeStorage.GetByFilterAsync(filter, orderBy, pageNumber, pageSize,
                cancellationToken);
            return employees.ToList();
        }

        public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            return await _employeeStorage.GetByIdAsync(id, cancellationToken);
        }

        public async Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _employeeStorage.DeleteAsync(id, cancellationToken);
        }
    }
}