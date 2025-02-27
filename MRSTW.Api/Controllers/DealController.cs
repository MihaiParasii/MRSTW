using Microsoft.AspNetCore.Mvc;
using MRSTW.BusinessLogicLayer.Contracts.Deal;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class DealController(DealService dealService) : ControllerBase
{
    [HttpGet("{pageSize:int},{pageCount:int}")]
    public async Task<ActionResult<List<DealResponse>>> Get(int pageSize, int pageCount)
    {
        if (pageCount <= 0)
        {
            return BadRequest("Page count must be a positive integer.");
        }

        if (pageSize <= 0)
        {
            return BadRequest("Page size must be a positive integer.");
        }

        var result = await dealService.GetPaginatedListAsync(pageSize, pageCount);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DealResponse>> Get(int id)
    {
        if (id < 0)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            var result = await dealService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromForm] CreateDealRequest request, [FromForm] List<IFormFile> files)
    {
        try
        {
            await dealService.CreateAsync(request);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromForm] UpdateDealRequest request, [FromForm] List<IFormFile> files)
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
            await dealService.UpdateAsync(request);
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
            await dealService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }
}
