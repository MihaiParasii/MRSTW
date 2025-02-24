using Microsoft.AspNetCore.Mvc;
using MRSTW.BusinessLogicLayer.Contracts.Deal;
using MRSTW.BusinessLogicLayer.Services;

namespace MRSTW.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DealController(DealService dealService) : ControllerBase
{
    [HttpGet("{pageSize:int},{pageCount:int}")]
    public async Task<ActionResult<List<DealResponse>>> Get(int pageSize, int pageCount)
    {
        var result = await dealService.GetPaginatedListAsync(pageSize, pageCount);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DealResponse>> Get(int id)
    {
        var result = await dealService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DealResponse>> Post([FromBody] CreateDealRequest request)
    {
        await dealService.CreateAsync(request);
        return Created();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateDealRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        await dealService.UpdateAsync(request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await dealService.DeleteAsync(id);
        return NoContent();
    }
}
