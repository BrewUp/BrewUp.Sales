using Brewup.Sales.ReadModel.Abstracts;
using Microsoft.Extensions.Logging;

namespace Brewup.Sales.ApplicationService.Abstracts;

public abstract class PurchaseBaseService
{
	protected readonly IPersister Persister;
	protected readonly ILogger Logger;

	protected PurchaseBaseService(IPersister persister,
		ILoggerFactory loggerFactory)
	{
		Persister = persister ?? throw new ArgumentNullException(nameof(persister));
		Logger = loggerFactory.CreateLogger(GetType());
	}
}