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
        var result = await subcategoryService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SubcategoryResponse>> Post([FromBody] CreateSubcategoryRequest request)
    {
        await subcategoryService.CreateAsync(request);
        return Created();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateSubcategoryRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        await subcategoryService.UpdateAsync(request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await subcategoryService.DeleteAsync(id);
        return NoContent();
    }
}
