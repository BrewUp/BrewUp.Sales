using Brewup.Modules.Sales.Domain.Entities;
using Brewup.Modules.Sales.Shared.CustomTypes;

namespace Brewup.Modules.Sales.Domain.Helpers;

public static class SalesCoreHelpers
{
	public static OrderRow ToEntity(this SalesOrderRow row) => new(row.RowId, row.BeerId, row.BeerName,
		row.QuantityOrdered, row.QuantityDelivered, row.UnitPrice);
}