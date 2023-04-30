using Brewup.Sales.Shared.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace Brewup.Sales.Muflone.Consumers.Commands;

public sealed class WithdrawalFromWarehouseConsumer : CommandConsumerBase<WithdrawalFromWarehouse>
{
	protected override ICommandHandlerAsync<WithdrawalFromWarehouse> HandlerAsync => default!;

	public WithdrawalFromWarehouseConsumer(IRepository repository,
		AzureServiceBusConfiguration azureServiceBusConfiguration,
		ILoggerFactory loggerFactory, ISerializer? messageSerializer = null) : base(azureServiceBusConfiguration, loggerFactory, messageSerializer)
	{
	}
}