using Domain.Models.Main;
using FluentValidation;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.BusinessLogicLayer.Validators;

public class SubcategoryValidator : AbstractValidator<Subcategory>
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

    private bool IsUniqueSubcategoryName(Subcategory subcategory)
    {
        if (string.IsNullOrWhiteSpace(subcategory.Name))
        {
            return false;
        }

        if (subcategory.CategoryId <= 0)
        {
            return false;
        }

        var subcategories = _subcategoryRepository.GetAllByCategoryIdAsync(subcategory.CategoryId).GetAwaiter()
            .GetResult();

        return subcategories.All(x =>
            !string.Equals(x.Name, subcategory.Name, StringComparison.CurrentCultureIgnoreCase));
    }

    private static bool IsValidName(string name)
    {
        return name.All(char.IsLetter);
    }
}
