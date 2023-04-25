using Brewup.Sales.Domain.Abstracts;
using Brewup.Sales.Shared.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace Brewup.Sales.Domain.CommandHandlers;

// This commandHandler must be registerd in Warehouse microservice. 
// I registerd here just for test purpose.
public class AskForBeersAvailabilityCommandHandler : CommandHandlerAsync<AskForBeersAvailability>
{
	public AskForBeersAvailabilityCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
	{
	}

	public override Task HandleAsync(AskForBeersAvailability command, CancellationToken cancellationToken = new())
	{
		return Task.CompletedTask;
	}
}