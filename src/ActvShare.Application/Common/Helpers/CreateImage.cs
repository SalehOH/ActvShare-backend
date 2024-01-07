using ActvShare.Application.Common.Responses;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Common.Helpers;

public class CreateImage : ICreateImage
{
    public async Task<ImageResponse> Create(IFormFile file, CancellationToken cancellationToken = default)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        var filePath = Path.Combine(directoryPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return new ImageResponse(fileName, file.FileName, file.Length);
    }

}