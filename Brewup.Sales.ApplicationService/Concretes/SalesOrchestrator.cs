using Brewup.Sales.ApplicationService.Abstracts;
using Brewup.Sales.ApplicationService.Sagas;
using Brewup.Sales.Shared.Commands;
using Brewup.Sales.Shared.Concretes;
using Brewup.Sales.Shared.CustomTypes;
using Brewup.Sales.Shared.Dtos;
using Microsoft.Extensions.Logging;

namespace Brewup.Sales.ApplicationService.Concretes;

internal sealed class SalesOrchestrator : ISalesOrchestrator
{
	private readonly ILogger _logger;
	private readonly SalesOrderSaga _salesOrderSaga;

	public SalesOrchestrator(ILoggerFactory loggerFactory,
		SalesOrderSaga salesOrderSaga)
	{
		_salesOrderSaga = salesOrderSaga ?? throw new ArgumentNullException(nameof(salesOrderSaga));
		_logger = loggerFactory.CreateLogger(GetType());
	}

	public async Task<string> AddOrderAsync(SalesOrderJson orderToAdd, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(orderToAdd.OrderId))
				orderToAdd.OrderId = Guid.NewGuid().ToString();

			// Verify that BeerId, CustomerId, etc. are valid

			// Start Saga here!
			var launchSalesOrder = new LaunchSalesOrderSaga(new OrderId(new Guid(orderToAdd.OrderId)), Guid.NewGuid(),
				new OrderNumber(orderToAdd.OrderNumber), new OrderDate(orderToAdd.OrderDate),
				new WarehouseId(new Guid(orderToAdd.WarehouseId)), new CustomerId(orderToAdd.CustomerId),
				new CustomerName(orderToAdd.CustomerName), new TotalAmount(orderToAdd.TotalAmount),
				orderToAdd.Rows.Select(r => new PurchaseOrderRow(new OrderId(new Guid(orderToAdd.OrderId)),
					new RowId(Guid.NewGuid().ToString()), new RowNumber(r.RowNumber), new BeerId(r.BeerId),
					new BeerName(r.BeerName), new QuantityOrdered(r.QuantityOrdered),
					new QuantityDelivered(r.QuantityDelivered), new UnitPrice(r.UnitPrice),
					new TotalAmount(r.UnitPrice * r.QuantityOrdered))));

			await _salesOrderSaga.StartedByAsync(launchSalesOrder);

			return orderToAdd.OrderId;
		}
		catch (Exception ex)
		{
			_logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
			throw;
		}
	}
}