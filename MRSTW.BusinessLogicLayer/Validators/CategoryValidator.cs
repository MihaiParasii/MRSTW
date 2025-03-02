using Domain.Models.Main;
using FluentValidation;

namespace MRSTW.BusinessLogicLayer.Validators;

public class CategoryValidator : AbstractValidator<CategoryModel>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100).WithMessage("Category name must not be empty!");
    }
}
