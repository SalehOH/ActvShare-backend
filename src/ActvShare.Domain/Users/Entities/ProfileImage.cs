using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Users.Entities;

public class ProfileImage: ImageEntity<ProfileImageId>
{
    
    private ProfileImage(
        ProfileImageId id,
        string storedFileName,
        string originalFileName, 
        string? contentType, 
        long fileSize)
        : base(id, storedFileName, originalFileName, contentType, fileSize)
    {
        
    }
    
    public static ProfileImage Create(
        string storedFileName, 
        string originalFileName, 
        string? contentType, 
        long fileSize
        )
    {
        return new ProfileImage(
            ProfileImageId.CreateUnique(),
            storedFileName, 
            originalFileName, 
            contentType, 
            fileSize);
    }
    
    private ProfileImage(){}
}