﻿using Brewup.Sales.Shared.CustomTypes;
using Muflone.Messages.Events;

namespace Brewup.Sales.Shared.DomainEvents;

public sealed class SalesOrderCreated : DomainEvent
{
	public readonly OrderId OrderId;
	public readonly OrderNumber OrderNumber;
	public readonly OrderDate OrderDate;

	public readonly CustomerId CustomerId;
	public readonly CustomerName CustomerName;

	public readonly TotalAmount TotalAmount;

	public readonly IEnumerable<SalesOrderRow> Rows;

	public SalesOrderCreated(OrderId aggregateId, Guid commitId, OrderNumber orderNumber, OrderDate orderDate,
		CustomerId customerId, CustomerName customerName, TotalAmount totalAmount, IEnumerable<SalesOrderRow> rows) :
		base(aggregateId, commitId)
	{
		OrderId = aggregateId;
		OrderNumber = orderNumber;
		OrderDate = orderDate;

		CustomerId = customerId;
		CustomerName = customerName;

		TotalAmount = totalAmount;

		Rows = rows;
	}
}