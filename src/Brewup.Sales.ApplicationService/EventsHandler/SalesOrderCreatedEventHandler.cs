using Brewup.Sales.ApplicationService.Abstracts;
using Brewup.Sales.ApplicationService.Sagas;
using Brewup.Sales.Shared.DomainEvents;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Saga.Persistence;

namespace Brewup.Sales.ApplicationService.EventsHandler;

public sealed class SalesOrderCreatedEventHandler : DomainEventHandlerAsync<SalesOrderCreated>
{
	private readonly ISalesOrderService _salesOrderService;
	private readonly ISagaRepository _sagaRepository;

	public SalesOrderCreatedEventHandler(ILoggerFactory loggerFactory,
		ISalesOrderService salesOrderService, ISagaRepository sagaRepository) : base(loggerFactory)
	{
		_salesOrderService = salesOrderService ?? throw new ArgumentNullException(nameof(salesOrderService));
		_sagaRepository = sagaRepository ?? throw new ArgumentNullException(nameof(sagaRepository));
	}

	public override async Task HandleAsync(SalesOrderCreated @event, CancellationToken cancellationToken = new())
	{
		try
		{
			var correlationId = new Guid(@event.UserProperties.FirstOrDefault(u => u.Key.Equals("CorrelationId")).Value.ToString()!);
			var sagaState = await _sagaRepository.GetByIdAsync<SalesSagaState>(correlationId);

			if (sagaState is null)
				return;

			await _salesOrderService.AddOrderAsync(@event, cancellationToken);
		}
		catch (Exception ex)
		{
			throw;
		}
	}
}