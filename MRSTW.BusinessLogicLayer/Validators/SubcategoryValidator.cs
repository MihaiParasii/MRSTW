using Domain.Models.Main;
using FluentValidation;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.BusinessLogicLayer.Validators;

public class SubcategoryValidator : AbstractValidator<SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public SubcategoryValidator(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;

        RuleFor(x => x).Must(IsUniqueSubcategoryName)
            .WithMessage("Subcategory name must be unique!");

        RuleFor(x => x.Name).NotEmpty().Length(1, 100)
            .WithMessage("Subcategory name must not be empty!");

        RuleFor(x => x.Name).Must(IsValidName)
            .WithMessage("Subcategory name can only contain letters!");

        RuleFor(x => x.CategoryId).NotEmpty();
    }

    private bool IsUniqueSubcategoryName(SubcategoryModel subcategoryModel)
    {
        if (string.IsNullOrWhiteSpace(subcategoryModel.Name))
        {
            return false;
        }

        if (subcategoryModel.CategoryId <= 0)
        {
            return false;
        }

        var subcategories = _subcategoryRepository.GetAllByCategoryIdAsync(subcategoryModel.CategoryId).GetAwaiter()
            .GetResult();

        return subcategories.All(x =>
            !string.Equals(x.Name, subcategoryModel.Name, StringComparison.CurrentCultureIgnoreCase));
    }

    private static bool IsValidName(string name)
    {
        return name.All(char.IsLetter);
    }
}
