using Brewup.Sales.Shared.Dtos;

namespace Brewup.Sales.ApplicationService.Sagas;

public class SalesSagaState
{
	public Guid SagaId { get; set; } = Guid.NewGuid();

	public SalesOrderJson Order { get; set; } = new();
	public bool AvailabilityChecked { get; set; } = false;
	public bool SalesOrderCreated { get; set; } = false;
}