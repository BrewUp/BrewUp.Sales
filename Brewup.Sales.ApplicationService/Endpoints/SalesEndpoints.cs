using Brewup.Sales.ApplicationService.Abstracts;
using Brewup.Sales.Shared.Concretes;
using Brewup.Sales.Shared.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Brewup.Sales.ApplicationService.Endpoints;

public static class SalesEndpoints
{
	public static async Task<IResult> HandleAddOrder(
		ISalesOrchestrator salesOrchestrator,
		IValidator<SalesOrderJson> validator,
		ValidationHandler validationHandler,
		SalesOrderJson body,
		CancellationToken cancellationToken
	)
	{
		await validationHandler.ValidateAsync(validator, body);
		if (!validationHandler.IsValid)
			return Results.BadRequest(validationHandler.Errors);

		await salesOrchestrator.AddOrderAsync(body, cancellationToken);

		return Results.Ok();
	}

	public static async Task<IResult> HandleGetOrders(
		ISalesOrchestrator salesOrchestrator,
		CancellationToken cancellationToken
	)
	{
		var salesOrders = await salesOrchestrator.GetSalesOrdersAsync(cancellationToken);

		return Results.Ok(salesOrders);
	}
}