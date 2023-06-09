﻿using Brewup.Sales.Shared.Commands;
using Brewup.Sales.Shared.Dtos;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace Brewup.Sales.ApplicationService.Sagas;

public class SalesOrderSaga : Saga<SalesSagaState>,
	ISagaStartedByAsync<LaunchSalesOrderSaga>
{
	public SalesOrderSaga(IServiceBus serviceBus,
		ISagaRepository repository,
		ILoggerFactory loggerFactory) : base(serviceBus, repository, loggerFactory)
	{
	}

	public async Task StartedByAsync(LaunchSalesOrderSaga command)
	{
		SagaState = new SalesSagaState
		{
			Order = new SalesOrderJson
			{
				OrderId = command.OrderId.Value.ToString(),
				OrderNumber = command.OrderNumber.Value,
				OrderDate = command.OrderDate.Value,

				WarehouseId = command.WarehouseId.Value.ToString(),

				CustomerId = command.CustomerId.Value,
				CustomerName = command.CustomerName.Value,

				TotalAmount = command.TotalAmount.Value,
				Rows = command.Rows.Select(r => new SalesOrderRowJson
				{
					RowNumber = r.RowNumber.Value,
					BeerId = r.BeerId.Value.ToString(),
					BeerName = r.BeerName.Value,
					QuantityOrdered = r.QuantityOrdered.Value,
					QuantityDelivered = r.QuantityDelivered.Value,
					UnitPrice = r.UnitPrice.Value
				}).ToList()
			}
		};

		// Save SagaState
		await Repository.SaveAsync(command.MessageId, SagaState);

		// I have to send the first command of the saga
		var askForBeersAvailability =
			new AskForBeersAvailability(command.WarehouseId, command.MessageId, command.Rows.Select(r => r.BeerId));
		await ServiceBus.SendAsync(askForBeersAvailability, CancellationToken.None);
	}
}