using Brewup.Sales.Shared.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace Brewup.Sales.Muflone.Consumers.Events;

public sealed class SalesOrderCreatedConsumer : DomainEventConsumerBase<SalesOrderCreated>
{
	protected override IEnumerable<IDomainEventHandlerAsync<SalesOrderCreated>> HandlersAsync { get; }

	public SalesOrderCreatedConsumer(IServiceProvider serviceProvider,
		AzureServiceBusConfiguration azureServiceBusConfiguration,
		ILoggerFactory loggerFactory,
		ISerializer? messageSerializer = null) : base(azureServiceBusConfiguration, loggerFactory, messageSerializer)
	{
		HandlersAsync = serviceProvider.GetServices<IDomainEventHandlerAsync<SalesOrderCreated>>();
	}
}