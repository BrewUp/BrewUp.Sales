using Brewup.Sales.Domain.Entities;
using Brewup.Sales.Shared.CustomTypes;

namespace Brewup.Sales.Domain.Helpers;

public static class SalesCoreHelpers
{
	public static OrderRow ToEntity(this SalesOrderRow row) => new(row.RowId, row.BeerId, row.BeerName,
		row.QuantityOrdered, row.QuantityDelivered, row.UnitPrice);
}