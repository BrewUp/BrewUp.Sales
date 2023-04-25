using Brewup.Sales.ApplicationService.Sagas;
using Brewup.Sales.Muflone;
using Brewup.Sales.ReadModel.MongoDb;
using Brewup.Sales.Shared.Concretes;
using Brewup.Sales.Shared.Configuration;
using Muflone.Eventstore;
using Muflone.Saga.Persistence.MongoDb;
using Serilog;

namespace BrewUp.Sales.Modules;

public class InfrastructureModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 99;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
		Log.Logger = new LoggerConfiguration()
		.WriteTo.File("Logs\\BreUpSales.log")
			.CreateLogger();

		builder.Services.AddScoped<ValidationHandler>();

		builder.Services.AddMongoDb(builder.Configuration.GetSection("BrewUp:MongoDbSettings").Get<MongoDbSettings>()!);
		builder.Services.AddEventstoreMongoDb(builder.Configuration.GetSection("BrewUp:MongoDbSettings").Get<MongoDbSettings>()!);
		builder.Services.AddMongoSagaStateRepository(new MongoSagaStateRepositoryOptions(builder.Configuration["BrewUp:MongoDbSettings:ConnectionString"]!,
			builder.Configuration["BrewUp:MongoDbSettings:DatabaseName"]!));

		builder.Services.AddMufloneEventStore(builder.Configuration["BrewUp:EventStoreSettings:ConnectionString"]!);
		builder.Services.AddMuflone(builder.Configuration.GetSection("BrewUp:ServiceBusSettings").Get<ServiceBusSettings>()!);

		builder.Services.AddScoped<SalesOrderSaga>();

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}