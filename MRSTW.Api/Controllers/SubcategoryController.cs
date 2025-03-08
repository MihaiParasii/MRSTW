using Microsoft.AspNetCore.Mvc;
using MRSTW.Api.UnitOfWork;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;

namespace MRSTW.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class SubcategoryController(IApiUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SubcategoryResponse>>> Get()
    {
        var result = await unitOfWork.SubcategoryService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubcategoryResponse>> Get(int id)
    {
        if (id < 0)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            var result = await unitOfWork.SubcategoryService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<SubcategoryResponse>> Post([FromBody] CreateSubcategoryRequest request)
    {
        var validationResult = await unitOfWork.CreateSubcategoryValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        try
        {
            await unitOfWork.SubcategoryService.CreateAsync(request);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] UpdateSubcategoryRequest request)
    {
        var validationResult = await unitOfWork.UpdateSubcategoryValidator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        
        try
        {
            await unitOfWork.SubcategoryService.UpdateAsync(request);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
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
            await unitOfWork.SubcategoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}
