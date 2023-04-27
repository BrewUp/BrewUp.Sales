using Brewup.Sales.Shared.DomainEvents;
using Brewup.Sales.Shared.Dtos;

namespace Brewup.Sales.ApplicationService.Abstracts;

public interface ISalesOrderService
{
	Task AddOrderAsync(SalesOrderCreated @event, CancellationToken cancellationToken);

	Task<IEnumerable<SalesOrderJson>> GetSalesOrdersAsync(CancellationToken cancellationToken);
}