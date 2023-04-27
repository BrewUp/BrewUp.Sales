using Azure.Messaging.ServiceBus;
using Brewup.Sales.Muflone.Consumers.Commands;
using Brewup.Sales.Muflone.Consumers.Events;
using Brewup.Sales.Shared.Commands;
using Brewup.Sales.Shared.Configuration;
using Brewup.Sales.Shared.DomainEvents;
using Brewup.Sales.Shared.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Transport.Azure;
using Muflone.Transport.Azure.Abstracts;
using Muflone.Transport.Azure.Factories;
using Muflone.Transport.Azure.Models;

namespace Brewup.Sales.Muflone;

public static class MufloneHelper
{
	public static IServiceCollection AddMuflone(this IServiceCollection services, ServiceBusSettings serviceBusSettings)
	{
		// I need to register IServiceBus and IEventBus due to Saga implementation
		services.AddSingleton(new ServiceBusClient(serviceBusSettings.ConnectionString));
		services.AddSingleton<IServiceBusSenderFactory, ServiceBusSenderFactory>();
		services.AddSingleton<IServiceBus, ServiceBus>();
		services.AddSingleton<IEventBus, ServiceBus>();

		var serviceProvider = services.BuildServiceProvider();
		var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
		var repository = serviceProvider.GetService<IRepository>();

		var azureBusConfiguration =
			new AzureServiceBusConfiguration(serviceBusSettings.ConnectionString, nameof(AskForBeersAvailability),
				serviceBusSettings.ClientId);

		var consumers = new List<IConsumer>
		{
			new AskForBeersAvailabilityConsumer(repository!, azureBusConfiguration with { TopicName = nameof(AskForBeersAvailability)}, loggerFactory!),
			new BeersAvailabilityAskedConsumer(serviceProvider, azureBusConfiguration with { TopicName = nameof(BeersAvailabilityAsked) }, loggerFactory!),

			new WithdrawalFromWarehouseConsumer(repository!, azureBusConfiguration with { TopicName = nameof(WithdrawalFromWarehouse)}, loggerFactory!),

			new CreateSalesOrderConsumer(repository!, azureBusConfiguration with { TopicName = nameof(CreateSalesOrder)}, loggerFactory!),
			new SalesOrderCreatedConsumer(serviceProvider, azureBusConfiguration with { TopicName = nameof(SalesOrderCreated)}, loggerFactory!),

			new BroadcastBeerWithdrawnConsumer(serviceProvider, azureBusConfiguration with { TopicName = nameof(BroadcastBeerWithdrawn)}, loggerFactory !)
		};

		services.AddMufloneTransportAzure(
			azureBusConfiguration with { TopicName = string.Empty }, consumers);

		return services;
	}
}