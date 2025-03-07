using AutoMapper;
using Domain.Models.Main;
using FluentValidation;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.DataAccessLayer.Services;

public class BusinessUnitOfWork(
    ICategoryRepository categoryRepository,
    ISubcategoryRepository subcategoryRepository,
    IDealRepository dealRepository,
    IMapper mapper,
    IValidator<DealModel> dealValidator,
    IValidator<CategoryModel> categoryValidator,
    IValidator<SubcategoryModel> subcategoryValidator,
    IRabbitMqService rabbitMqService)
    : IBusinessUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; } = categoryRepository;
    public ISubcategoryRepository SubcategoryRepository { get; } = subcategoryRepository;
    public IDealRepository DealRepository { get; } = dealRepository;
    public IMapper Mapper { get; } = mapper;
    public IValidator<DealModel> DealValidator { get; } = dealValidator;
    public IValidator<CategoryModel> CategoryValidator { get; } = categoryValidator;
    public IValidator<SubcategoryModel> SubcategoryValidator { get; } = subcategoryValidator;
    public IRabbitMqService RabbitMqService { get; } = rabbitMqService;
}
