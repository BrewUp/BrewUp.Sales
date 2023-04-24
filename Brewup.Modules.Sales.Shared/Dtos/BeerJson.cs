namespace Brewup.Modules.Sales.Shared.Dtos;

public record BeerJson(string BeerId, string BeerName, double Stock, double Availability, double SalesCommitted);