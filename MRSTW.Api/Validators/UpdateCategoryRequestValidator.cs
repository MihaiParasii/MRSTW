using FluentValidation;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.Api.Validators;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100)
            .WithMessage("Category name must not be empty!");

        RuleFor(x => x.Id).GreaterThanOrEqualTo(0)
            .WithMessage("Id must not be greater than 0!");
    }
}
