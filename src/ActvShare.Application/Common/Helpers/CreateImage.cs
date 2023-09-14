using ActvShare.Application.Common.Responses;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Common.Helpers;

internal class CreateImage
{
    public static async Task<ImageResponse> Create(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine("wwwroot", "images", fileName);
        
        await file.CopyToAsync(File.Create(filePath));

        return new ImageResponse(fileName, file.FileName, file.Length);
    }
}