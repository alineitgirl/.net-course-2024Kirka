using System.Linq.Expressions;
using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private ClientService _clientService;

    public ClientsController(ClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("get_clients_by_id")]
    public async Task<IActionResult> GetClientsByIdAsync([FromQuery] Guid clientId, CancellationToken cancellationToken)
    {
        var response = await _clientService.GetByIdAsync(clientId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("get_clients_by_filter")]
    public async Task<IActionResult> SearchClients([FromQuery] ClientDto clientDto, CancellationToken cancellationToken,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        Expression<Func<Client, bool>> filter = client =>
            (string.IsNullOrEmpty(clientDto.FullName) || client.FirstName.Contains(clientDto.FullName)) &&
            (string.IsNullOrEmpty(clientDto.FullName) || client.LastName.Contains(clientDto.FullName)) &&
            (!clientDto.DateOfBirth.HasValue || client.DateOfBirth.Date == clientDto.DateOfBirth.Value.Date) &&
            (string.IsNullOrEmpty(clientDto.PhoneNumber) || client.PhoneNumber.Contains(clientDto.PhoneNumber)) &&
            (string.IsNullOrEmpty(clientDto.Address) || client.Adress.Contains(clientDto.Address)) &&
            (clientDto.Age == 0 || client.Age == clientDto.Age);
        
        Func<IQueryable<Client>, IOrderedQueryable<Client>> orderBy = query => query.OrderBy(c => c.FirstName);
        
        var clients = await _clientService.GetByFilterAsync(filter, orderBy, pageNumber, pageSize, cancellationToken);

        return Ok(clients);
    }

    [HttpPost("create_client")]
    public async Task<IActionResult> AddClientAsync([FromBody] ClientDto clientDto, CancellationToken cancellationToken)
    {
        await _clientService.AddClientAsync(clientDto, cancellationToken);
        return Ok();
    }

    [HttpPut("update_client")]
    public async Task<IActionResult> UpdateClientAsync([FromQuery] Guid id, ClientDto clientDto,
        CancellationToken cancellationToken)
    {
        await _clientService.UpdateClientAsync(id, clientDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("delete_client")]
    public async Task<IActionResult> DeleteClientAsync([FromQuery] Guid clientId, CancellationToken cancellationToken)
    {
        await _clientService.DeleteClientAsync(clientId, cancellationToken);
        return Ok();
    }
}