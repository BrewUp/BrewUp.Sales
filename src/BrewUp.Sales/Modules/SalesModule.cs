using Brewup.Sales.ApplicationService;
using Brewup.Sales.ApplicationService.Endpoints;

namespace BrewUp.Sales.Modules;

public class SalesModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		return builder.Services.AddSalesModule();
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		var mapGroup = endpoints.MapGroup("v1/sales")
			.WithTags("Sales");

		mapGroup.MapPost("", SalesEndpoints.HandleAddOrder)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("AddOrder");

		mapGroup.MapGet("", SalesEndpoints.HandleGetOrders)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetSalesOrders");

		return endpoints;
	}
}