using Brewup.Sales.Shared.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace Brewup.Sales.Muflone.Consumers.Events;

public sealed class BroadcastWarehouseCreatedConsumer : IntegrationEventConsumerBase<BroadcastWarehouseCreated>
{
	protected override IEnumerable<IIntegrationEventHandlerAsync<BroadcastWarehouseCreated>> HandlersAsync { get; }

	public BroadcastWarehouseCreatedConsumer(IServiceProvider serviceProvider,
		AzureServiceBusConfiguration azureServiceBusConfiguration,
		ILoggerFactory loggerFactory,
		ISerializer? messageSerializer = null) : base(azureServiceBusConfiguration, loggerFactory, messageSerializer)
	{
		HandlersAsync = serviceProvider.GetServices<IIntegrationEventHandlerAsync<BroadcastWarehouseCreated>>();
	}
}