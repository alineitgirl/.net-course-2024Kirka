using System.Linq.Expressions;
using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private EmployeeService _employeeService;

    public EmployeesController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("get_employees_by_id")]
    public async Task<IActionResult> GetEmployeesByIdAsync([FromQuery] Guid employeeId, CancellationToken cancellationToken)
    {
        var response = await _employeeService.GetByIdAsync(employeeId, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("get_employees_by_filter")]
    public async Task<IActionResult> SearchEmployees([FromQuery] EmployeeDto employeeDto, CancellationToken cancellationToken,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        Expression<Func<Employee, bool>> filter = employee =>
            (string.IsNullOrEmpty(employeeDto.FullName) || employee.FirstName.Contains(employeeDto.FullName)) &&
            (string.IsNullOrEmpty(employeeDto.FullName) || employee.LastName.Contains(employeeDto.FullName)) &&
            (string.IsNullOrEmpty(employeeDto.PhoneNumber) || employee.PhoneNumber.Contains(employeeDto.PhoneNumber)) &&
            (string.IsNullOrEmpty(employeeDto.Adress) || employee.Adress.Contains(employeeDto.Adress));
        
        Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = query => query.OrderBy(c => c.FirstName);
        
        var employees = await _employeeService.GetByFilterAsync(filter, orderBy, pageNumber, pageSize, cancellationToken);

        return Ok(employees);
    }

    [HttpPost("create_employee")]
    public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeDto employeeDto, CancellationToken cancellationToken)
    {
        await _employeeService.AddEmployeeAsync(employeeDto, cancellationToken);
        return Ok();
    }

    [HttpPut("update_employee")]
    public async Task<IActionResult> UpdateEmployeeAsync([FromQuery] Guid id, EmployeeDto employeeDto,
        CancellationToken cancellationToken)
    {
        await _employeeService.UpdateEmployeeAsync(id, employeeDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("delete_employee")]
    public async Task<IActionResult> DeleteClientAsync([FromQuery] Guid employeeId, CancellationToken cancellationToken)
    {
        await _employeeService.DeleteEmployeeAsync(employeeId, cancellationToken);
        return Ok();
    }
}