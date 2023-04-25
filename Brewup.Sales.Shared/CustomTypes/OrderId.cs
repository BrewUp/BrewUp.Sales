using Muflone.Core;

namespace Brewup.Sales.Shared.CustomTypes;

public sealed class OrderId : DomainId
{
	public OrderId(Guid value) : base(value)
	{
	}
}