﻿using Brewup.Sales.Shared.CustomTypes;
using Brewup.Sales.Shared.Dtos;

namespace Brewup.Sales.Shared.Helpers;

public static class SalesConvertHelper
{
	public static BeerToDrawn ToBeerToDrawn(this SalesOrderRowJson json) =>
		new(new BeerId(json.BeerId), new Quantity(json.QuantityOrdered), new Stock(0), new Availability(0));

	public static SalesOrderRow ToSalesOrderRow(this SalesOrderRowJson json) =>
		new(new RowId(json.RowId),
			new BeerId(json.BeerId),
			new BeerName(json.BeerName),
			new QuantityOrdered(json.QuantityOrdered),
			new QuantityDelivered(json.QuantityDelivered),
			new UnitPrice(json.UnitPrice));
}