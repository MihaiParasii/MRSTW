using FluentValidation;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts.Category;
using MRSTW.BusinessLogicLayer.Contracts.Deal;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.Api.UnitOfWork;

public class ApiUnitOfWork(
    ICategoryService categoryService,
    IDealService dealService,
    ISubcategoryService subcategoryService,
    IValidator<CreateCategoryRequest> createCategoryValidator,
    IValidator<UpdateCategoryRequest> updateCategoryValidator,
    IValidator<CreateSubcategoryRequest> createSubcategoryValidator,
    IValidator<UpdateSubcategoryRequest> updateSubcategoryValidator,
    IValidator<CreateDealRequest> createDealValidator,
    IValidator<UpdateDealRequest> updateDealValidator)
    : IApiUnitOfWork
{
    public ICategoryService CategoryService { get; } = categoryService;
    public IDealService DealService { get; } = dealService;
    public ISubcategoryService SubcategoryService { get; } = subcategoryService;
    public IValidator<CreateCategoryRequest> CreateCategoryValidator { get; } = createCategoryValidator;
    public IValidator<UpdateCategoryRequest> UpdateCategoryValidator { get; } = updateCategoryValidator;
    public IValidator<CreateSubcategoryRequest> CreateSubcategoryValidator { get; } = createSubcategoryValidator;
    public IValidator<UpdateSubcategoryRequest> UpdateSubcategoryValidator { get; } = updateSubcategoryValidator;
    public IValidator<CreateDealRequest> CreateDealValidator { get; } = createDealValidator;
    public IValidator<UpdateDealRequest> UpdateDealValidator { get; } = updateDealValidator;
}
