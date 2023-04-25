using Brewup.Sales.ApplicationService.Abstracts;
using Brewup.Sales.ApplicationService.Concretes;
using Brewup.Sales.ApplicationService.EventsHandler;
using Brewup.Sales.ApplicationService.Sagas;
using Brewup.Sales.ApplicationService.Validators;
using Brewup.Sales.Shared.DomainEvents;
using Brewup.Sales.Shared.IntegrationEvents;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Messages.Events;

namespace Brewup.Sales.ApplicationService;

public static class SalesHelper
{
	public static IServiceCollection AddSalesModule(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddValidatorsFromAssemblyContaining<SalesOrderValidator>();

		services.AddScoped<ISalesOrderService, SalesOrderService>();
		services.AddScoped<ISalesOrchestrator, SalesOrchestrator>();

		services.AddScoped<IDomainEventHandlerAsync<BeersAvailabilityAsked>, SalesOrderSaga>();
		services.AddScoped<IIntegrationEventHandlerAsync<BroadcastBeerWithdrawn>, SalesOrderSaga>();
		services.AddScoped<IDomainEventHandlerAsync<SalesOrderCreated>, SalesOrderSaga>();
		services.AddScoped<IDomainEventHandlerAsync<SalesOrderCreated>, SalesOrderCreatedEventHandler>();

		return services;
	}
}