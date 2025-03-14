using FluentValidation;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.Api.Validators;

public class UpdateSubcategoryRequestValidator : AbstractValidator<UpdateSubcategoryRequest>
{
    public UpdateSubcategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100)
            .WithMessage("Subcategory name must be not null!");

        RuleFor(x => x.CategoryId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid category ID.");

        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
