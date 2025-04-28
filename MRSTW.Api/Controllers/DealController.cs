using Microsoft.AspNetCore.Mvc;
using MRSTW.Api.Contracts;
using MRSTW.Api.UnitOfWork;
using MRSTW.BusinessLogicLayer.Contracts.Deal;
using Microsoft.AspNetCore.Authorization;

namespace MRSTW.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/v1")]

[AllowAnonymous]
public class DealController(IApiUnitOfWork unitOfWork) : ControllerBase
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

        var result = await unitOfWork.DealService.GetPaginatedListAsync(pageSize, pageCount);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    
    [AllowAnonymous]
    public async Task<ActionResult<DealResponse>> Get(int id)
    {
        if (id < 0)
        {
            return BadRequest("The id can't be negative");
        }

        try
        {
            var result = await unitOfWork.DealService.GetByIdAsync(id);
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
        var validationResult = await unitOfWork.CreateDealValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors[0].ToString());
        }

        try
        {
            // TODO Get UserId
            var userId = Guid.Empty;
            var response = await unitOfWork.AmazonS3Service.UploadFilesAsync(new UploadFilesRequest(files, userId));
            await unitOfWork.DealService.CreateAsync(request, response.UploadedFilePaths);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromForm] UpdateDealRequest request, [FromForm] List<IFormFile> files)
    {
        var validationResult = await unitOfWork.UpdateDealValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        try
        {
            var deal = await unitOfWork.DealService.GetByIdAsync(request.Id);
            await unitOfWork.AmazonS3Service.DeleteFilesAsync(deal.PhotoPaths.ToList());

            // TODO Get UserId
            var userId = Guid.Empty;
            var response = await unitOfWork.AmazonS3Service.UploadFilesAsync(new UploadFilesRequest(files, userId));
            await unitOfWork.DealService.UpdateAsync(request, response.UploadedFilePaths);
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
            var deal = await unitOfWork.DealService.GetByIdAsync(id);
            await unitOfWork.AmazonS3Service.DeleteFilesAsync(deal.PhotoPaths.ToList());
            await unitOfWork.DealService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("upload-files")]
    public async Task<ActionResult<UploadFilesResponse>> UploadFiles([FromForm] List<IFormFile> files)
    {
        return await unitOfWork.AmazonS3Service.UploadFilesAsync(new UploadFilesRequest(files, new Guid()));
    }

    [HttpPost("delete-files")]
    public async Task<ActionResult> DeleteFiles([FromBody] List<string> filePaths)
    {
        await unitOfWork.AmazonS3Service.DeleteFilesAsync(filePaths);
        return NoContent();
    }
}
