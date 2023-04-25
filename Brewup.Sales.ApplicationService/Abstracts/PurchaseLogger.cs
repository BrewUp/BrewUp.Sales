using Microsoft.Extensions.Logging;

namespace Brewup.Sales.ApplicationService.Abstracts;

public sealed class PurchaseLogger
{
	public readonly ILogger Logger;

	public PurchaseLogger(ILoggerFactory loggerFactory)
	{
		Logger = loggerFactory.CreateLogger(GetType());
	}
}