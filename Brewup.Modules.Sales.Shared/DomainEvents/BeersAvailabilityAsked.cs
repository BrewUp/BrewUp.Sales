﻿using Brewup.Modules.Sales.Shared.CustomTypes;
using Muflone.Messages.Events;

namespace Brewup.Modules.Sales.Shared.DomainEvents;

public sealed class BeersAvailabilityAsked : DomainEvent
{
	public readonly WarehouseId WarehouseId;

	public readonly IEnumerable<BeerAvailability> Availabilities;


	public BeersAvailabilityAsked(WarehouseId aggregateId, Guid correlationId,
		IEnumerable<BeerAvailability> availabilities) : base(aggregateId, correlationId)
	{
		WarehouseId = aggregateId;
		Availabilities = availabilities;
	}
}