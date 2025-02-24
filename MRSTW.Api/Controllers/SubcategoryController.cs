using Microsoft.AspNetCore.Mvc;
using MRSTW.BusinessLogicLayer.Contracts.Subcategory;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SubcategoryController(SubcategoryService subcategoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SubcategoryResponse>>> Get()
    {
        var result = await subcategoryService.GetAllAsync();
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
            var result = await subcategoryService.GetByIdAsync(id);
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
        try
        {
            await subcategoryService.CreateAsync(request);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateSubcategoryRequest request)
    {
        if (id < 0)
        {
            return BadRequest("The id can't be negative");
        }

        if (id != request.Id)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            await subcategoryService.UpdateAsync(request);
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
            await subcategoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}
