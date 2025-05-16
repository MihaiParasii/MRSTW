using FluentValidation;
using MRSTW.Api.Services;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts.Category;
using MRSTW.BusinessLogicLayer.Contracts.Deal;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.Api.UnitOfWork;

public interface IApiUnitOfWork
{
    public ICategoryService CategoryService { get; }
    public IDealService DealService { get; }
    public ISubcategoryService SubcategoryService { get; }
    public IAuthService UserService { get; }

    public IValidator<CreateCategoryRequest> CreateCategoryValidator { get; }
    public IValidator<UpdateCategoryRequest> UpdateCategoryValidator { get; }

    public IValidator<CreateSubcategoryRequest> CreateSubcategoryValidator { get; }
    public IValidator<UpdateSubcategoryRequest> UpdateSubcategoryValidator { get; }

    public IValidator<CreateDealRequest> CreateDealValidator { get; }
    public IValidator<UpdateDealRequest> UpdateDealValidator { get; }
    
    public AmazonS3Service AmazonS3Service { get; }
}
