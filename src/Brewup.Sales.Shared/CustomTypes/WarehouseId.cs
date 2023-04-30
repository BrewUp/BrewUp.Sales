using Muflone.Core;

namespace Brewup.Sales.Shared.CustomTypes;

public sealed class WarehouseId : DomainId
{
	public WarehouseId(Guid value) : base(value)
	{
	}
}