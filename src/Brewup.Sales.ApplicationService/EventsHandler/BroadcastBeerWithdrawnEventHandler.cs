using Brewup.Sales.ApplicationService.Sagas;
using Brewup.Sales.Shared.Commands;
using Brewup.Sales.Shared.CustomTypes;
using Brewup.Sales.Shared.Helpers;
using Brewup.Sales.Shared.IntegrationEvents;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;
using Muflone.Saga.Persistence;

namespace Brewup.Sales.ApplicationService.EventsHandler;

public sealed class BroadcastBeerWithdrawnEventHandler : IntegrationEventHandlerAsync<BroadcastBeerWithdrawn>
{
	private readonly IServiceBus _serviceBus;
	private readonly ISagaRepository _sagaRepository;

	public BroadcastBeerWithdrawnEventHandler(ILoggerFactory loggerFactory,
		IServiceBus serviceBus,
		ISagaRepository sagaRepository) : base(loggerFactory)
	{
		_serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
		_sagaRepository = sagaRepository ?? throw new ArgumentNullException(nameof(sagaRepository));
	}

	public override async Task HandleAsync(BroadcastBeerWithdrawn @event, CancellationToken cancellationToken = new())
	{
		try
		{
			var correlationId = new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
			var sagaState = await _sagaRepository.GetByIdAsync<SalesSagaState>(correlationId);

			if (sagaState is null)
				return;

			var createSalesOrder = new CreateSalesOrder(
				new OrderId(new Guid(sagaState.Order.OrderId)),
				correlationId,
				new OrderNumber(sagaState.Order.OrderNumber),
				new OrderDate(sagaState.Order.OrderDate),
				new CustomerId(sagaState.Order.CustomerId),
				new CustomerName(sagaState.Order.CustomerName),
				new TotalAmount(sagaState.Order.TotalAmount),
				sagaState.Order.Rows.Select(r => r.ToSalesOrderRow()));

			await _serviceBus.SendAsync(createSalesOrder, cancellationToken);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			throw;
		}
	}
}