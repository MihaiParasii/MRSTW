using Domain.Models.Main;
using FluentValidation;
using MRSTW.BusinessLogicLayer.Common.Interfaces;

namespace MRSTW.BusinessLogicLayer.Validators;

public class CategoryValidator : AbstractValidator<CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name).NotEmpty().Length(1, 100)
            .WithMessage("Category name must not be empty!");

        RuleFor(x => x).Must(IsUniqueCategoryName)
            .WithMessage("Category name must be unique!");

        RuleFor(x => x.Name).Must(IsValidName)
            .WithMessage("Category name can only contain letters and white spaces!");
    }

    private bool IsUniqueCategoryName(CategoryModel categoryModel)
    {
        if (string.IsNullOrWhiteSpace(categoryModel.Name))
        {
            return false;
        }

        if (categoryModel.Id < 0)
        {
            return false;
        }

        var categories = _categoryRepository.GetAllAsync().GetAwaiter().GetResult();


        foreach (var x in categories)
        {
            if (string.Equals(x.Name, categoryModel.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsValidName(string name)
    {
        return name.All(c => char.IsLetter(c) || c == ' ');
    }
}