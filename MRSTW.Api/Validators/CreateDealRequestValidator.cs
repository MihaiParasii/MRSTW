using FluentValidation;
using MRSTW.BusinessLogicLayer.Contracts.Deal;

namespace MRSTW.Api.Validators;

public class CreateDealRequestValidator : AbstractValidator<CreateDealRequest>
{
    public CreateDealRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().Length(1, 100)
            .WithMessage("Title must be at least 1 character and at most 100 characters");

        RuleFor(x => x.Description).NotEmpty().Length(1, 1000)
            .WithMessage("Description must be at least 1 character and at most 1000 characters");

        RuleFor(x => x.SubcategoryId).NotEmpty()
            .WithMessage("SubcategoryId is required");
    }
}
