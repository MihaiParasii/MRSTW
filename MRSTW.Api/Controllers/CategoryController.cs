using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MRSTW.BusinessLogicLayer.Contracts.Category;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class CategoryController(
    CategoryService categoryService,
    IValidator<CreateCategoryRequest> createCategoryValidator,
    IValidator<UpdateCategoryRequest> updateCategoryValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> Get()
    {
        var result = await categoryService.GetAllAsync();
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
            var result = await categoryService.GetByIdAsync(id);
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
        var validationResult = await createCategoryValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        try
        {
            await categoryService.CreateAsync(request);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateCategoryRequest request)
    {
        var validationResult = await updateCategoryValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
       

        if (id != request.Id)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            await categoryService.UpdateAsync(request);
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
            await categoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}
