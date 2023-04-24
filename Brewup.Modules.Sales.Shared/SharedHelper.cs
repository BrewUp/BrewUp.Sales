using Brewup.Modules.Sales.Shared.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace Brewup.Modules.Sales.Shared;

public static class SharedHelper
{
	public static IServiceCollection AddAsharedServices(this IServiceCollection services)
	{
		services.AddSingleton<ValidationHandler>();

		return services;
	}
}