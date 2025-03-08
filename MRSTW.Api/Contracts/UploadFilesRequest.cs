namespace MRSTW.Api.Contracts;

public record UploadFilesRequest(List<IFormFile> Files, Guid UserId);
