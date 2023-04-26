using Brewup.Sales.ApplicationService;
using Brewup.Sales.ApplicationService.Endpoints;

namespace BrewUp.Sales.Modules;

public class SalesModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.AddSalesModule();

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		var mapGroup = endpoints.MapGroup("v1/sales")
			.WithTags("Sales");

		mapGroup.MapPost("", SalesEndpoints.HandleAddOrder)
			.Produces(StatusCodes.Status400BadRequest)
			.WithName("AddOrder");

		return endpoints;
	}
}