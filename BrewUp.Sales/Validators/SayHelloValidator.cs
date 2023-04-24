using BrewUp.Sales.Models;
using FluentValidation;

namespace BrewUp.Sales.Validators
{
	public class SayHelloValidator : AbstractValidator<HelloRequest>
	{
		public SayHelloValidator()
		{
			RuleFor(h => h.Name).NotEmpty().MaximumLength(50);
		}
	}
}