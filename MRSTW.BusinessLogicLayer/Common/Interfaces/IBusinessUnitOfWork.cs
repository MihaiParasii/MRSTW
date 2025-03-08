using AutoMapper;
using Domain.Models.Main;
using FluentValidation;

namespace MRSTW.BusinessLogicLayer.Common.Interfaces;

public interface IBusinessUnitOfWork
{
    public ICategoryRepository CategoryRepository { get;}
    public ISubcategoryRepository SubcategoryRepository { get; }
    public IDealRepository DealRepository { get; }
    
    public IMapper Mapper { get; }
    public IValidator<DealModel> DealValidator  { get; }
    public IValidator<CategoryModel> CategoryValidator  { get; }
    public IValidator<SubcategoryModel> SubcategoryValidator  { get; }
    
    public IRabbitMqService RabbitMqService { get; }
}
