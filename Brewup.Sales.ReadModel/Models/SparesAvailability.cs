using Brewup.Modules.Sales.Shared.CustomTypes;
using Brewup.Sales.ReadModel.Abstracts;

namespace Brewup.Sales.ReadModel.Models;

public class SparesAvailability : ModelBase
{
	public double Stock { get; private set; }
	public double Availability { get; private set; }
	public double ProductionCommitted { get; private set; }
	public double SalesCommitted { get; private set; }
	public double SupplierOrdered { get; private set; }

	protected SparesAvailability()
	{ }

	public static SparesAvailability CreateSparesAvailability(SpareId spareId, Stock stock, Availability availability,
		ProductionCommitted productionCommitted, SalesCommitted salesCommitted, SupplierOrdered supplierOrdered)
		=> new(spareId.Value.ToString(), stock.Value, availability.Value, productionCommitted.Value,
			salesCommitted.Value, supplierOrdered.Value);

	private SparesAvailability(string spareId, double stock, double availability, double productionCommitted,
		double salesCommitted, double supplierOrdered)
	{
		Id = spareId;
		Stock = stock;
		Availability = availability;
		ProductionCommitted = productionCommitted;
		SalesCommitted = salesCommitted;
		SupplierOrdered = supplierOrdered;
	}

	//public SpareAvailabilityJson ToJson() => new(Id, Stock, Availability, ProductionCommitted, SalesCommitted, SupplierOrdered);
}