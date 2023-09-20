using ActvShare.Application.Common.Responses;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Common.Helpers;

public class CreateImage: ICreateImage
{
    public async Task<ImageResponse> Create(IFormFile file, CancellationToken cancellationToken = default)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine("wwwroot", "images", fileName);
        
        await file.CopyToAsync(File.Create(filePath), cancellationToken);

        return new ImageResponse(fileName, file.FileName, file.Length);
    }
    
}