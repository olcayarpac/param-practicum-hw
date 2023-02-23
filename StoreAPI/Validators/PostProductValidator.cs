using FluentValidation;
using StoreAPI.Models;

namespace StoreAPI.Validators
{
    public class PostProductValidator : AbstractValidator<Product>
    {
        public PostProductValidator()
        {
            RuleFor(product => product.Category).NotEmpty();
            RuleFor(product => product.Price).NotEmpty().GreaterThan(0);
            RuleFor(product => product.Id).Empty();
            RuleFor(product => product.ProductName).NotEmpty();
        }
    }
}