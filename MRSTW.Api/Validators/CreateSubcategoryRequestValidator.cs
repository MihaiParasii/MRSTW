using FluentValidation;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.Api.Validators;

public class CreateSubcategoryRequestValidator : AbstractValidator<CreateSubcategoryRequest>
{
    public CreateSubcategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100)
            .WithMessage("Subcategory name must be not null!");

        RuleFor(x => x.CategoryId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid category ID.");

        RuleFor(x => x.Name).Must(IsValidName)
            .WithMessage("Subcategory name can only contain letters!");


        RuleFor(x => x.CategoryId).NotEmpty();
    }

    private static bool IsValidName(string name)
    {
        return name.All(char.IsLetter);
    }
}
