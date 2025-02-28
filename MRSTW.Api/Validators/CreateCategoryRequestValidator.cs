using FluentValidation;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.Api.Validators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100)
            .WithMessage("Category name must not be empty!");
    }
}
