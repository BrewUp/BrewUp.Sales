using Brewup.Sales.Shared.DomainEvents;

namespace Brewup.Sales.ApplicationService.Abstracts;

public interface ISalesOrderService
{
	Task AddOrderAsync(SalesOrderCreated @event, CancellationToken cancellationToken);
}