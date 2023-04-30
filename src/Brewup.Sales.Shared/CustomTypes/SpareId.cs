using Muflone.Core;

namespace Brewup.Sales.Shared.CustomTypes;

public class SpareId : DomainId
{
	public SpareId(Guid value) : base(value)
	{
	}
}