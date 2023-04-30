using Brewup.Sales.ApplicationService.Sagas;
using Brewup.Sales.Shared.Commands;
using Brewup.Sales.Shared.DomainEvents;
using Brewup.Sales.Shared.Helpers;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;
using Muflone.Saga.Persistence;

namespace Brewup.Sales.ApplicationService.EventsHandler;

public sealed class BeersAvailabilityAskedEventHandler : DomainEventHandlerAsync<BeersAvailabilityAsked>
{
	private readonly IServiceBus _serviceBus;
	private readonly ISagaRepository _sagaRepository;

	public BeersAvailabilityAskedEventHandler(ILoggerFactory loggerFactory,
		IServiceBus serviceBus,
		ISagaRepository sagaRepository) : base(loggerFactory)
	{
		_serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
		_sagaRepository = sagaRepository ?? throw new ArgumentNullException(nameof(sagaRepository));
	}

	public override async Task HandleAsync(BeersAvailabilityAsked @event, CancellationToken cancellationToken = new())
	{
		try
		{
			var correlationId = new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
			var sagaState = await _sagaRepository.GetByIdAsync<SalesSagaState>(correlationId);

			if (sagaState is null || sagaState.AvailabilityChecked)
				return;

			// Verify availability
			// With availability we create order
			// Without availability we send a message to the customer

			var withdrawalFromWarehouse = new WithdrawalFromWarehouse(@event.WarehouseId, correlationId,
				sagaState.Order.Rows.Select(r => r.ToBeerToDrawn()));

			await _serviceBus.SendAsync(withdrawalFromWarehouse, cancellationToken);

			// Update SagaState
			sagaState.AvailabilityChecked = true;
			await _sagaRepository.SaveAsync(correlationId, sagaState);
		}
		catch (Exception ex)
		{

			throw;
		}
	}
}