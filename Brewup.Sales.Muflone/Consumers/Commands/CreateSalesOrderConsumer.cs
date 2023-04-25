using Brewup.Sales.Domain.CommandHandlers;
using Brewup.Sales.Shared.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace Brewup.Sales.Muflone.Consumers.Commands;

public sealed class CreateSalesOrderConsumer : CommandConsumerBase<CreateSalesOrder>
{
	protected override ICommandHandlerAsync<CreateSalesOrder> HandlerAsync { get; }

	public CreateSalesOrderConsumer(IRepository repository,
		AzureServiceBusConfiguration azureServiceBusConfiguration,
		ILoggerFactory loggerFactory,
		ISerializer? messageSerializer = null) : base(azureServiceBusConfiguration, loggerFactory, messageSerializer)
	{
		HandlerAsync = new CreateSalesOrderCommandHandler(repository, loggerFactory);
	}
}