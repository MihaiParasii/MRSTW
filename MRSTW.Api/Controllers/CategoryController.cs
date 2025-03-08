using Microsoft.AspNetCore.Mvc;
using MRSTW.Api.UnitOfWork;
using MRSTW.BusinessLogicLayer.Contracts.Category;

namespace MRSTW.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class CategoryController(IApiUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> Get()
    {
        var result = await unitOfWork.CategoryService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> Get(int id)
    {
        if (id < 0)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            var result = await unitOfWork.CategoryService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Post([FromBody] CreateCategoryRequest request)
    {
        var validationResult = await unitOfWork.CreateCategoryValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        try
        {
            await unitOfWork.CategoryService.CreateAsync(request);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] UpdateCategoryRequest request)
    {
        var validationResult = await unitOfWork.UpdateCategoryValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        
        try
        {
            await unitOfWork.CategoryService.UpdateAsync(request);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (id < 0)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            await unitOfWork.CategoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}
