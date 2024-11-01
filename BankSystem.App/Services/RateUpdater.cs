using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class RateUpdater
    {
        private readonly IClientStorage _clientStorage;
        private readonly double _rate;
        public RateUpdater(IClientStorage clientStorage, double rate)
        {
            _clientStorage = clientStorage;
            _rate = rate;
        }

        public async Task UpdateRateAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                var today = DateTime.UtcNow.Date;
                var clientsToUpdate = _clientStorage.GetByFilterAsync(cl => cl.CreatedOn.Date == today.AddMonths(-1),
                    cl => cl.OrderBy(c => c.Age), 1, 100, cancellationToken);
            
                foreach (var client in clientsToUpdate.Result)
                {
                    var clientWithAccounts = await _clientStorage.GetClientWithAccountsAsync(client.Id, cancellationToken);
                    var accountOfClient = clientWithAccounts.Accounts.FirstOrDefault(); 
                    if (accountOfClient != null)
                    {
                        var newAccount = new Account() { Amount = accountOfClient.Amount * (1+_rate),
                            CurrencyName = accountOfClient.CurrencyName};
                        await _clientStorage.UpdateAccountAsync(client.Id, accountOfClient, newAccount, cancellationToken);
                    }
                }
            }
        }
    }
}