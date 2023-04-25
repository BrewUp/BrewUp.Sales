using Brewup.Sales.Shared.Dtos;

namespace Brewup.Sales.ApplicationService.Abstracts;

public interface ISalesOrchestrator
{
	Task<string> AddOrderAsync(SalesOrderJson orderToAdd, CancellationToken cancellationToken);
}