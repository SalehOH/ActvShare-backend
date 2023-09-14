using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Posts.ValueObjects;

namespace ActvShare.Domain.Posts.Entities;

public class PostImage: ImageEntity<PostImageId>
{
    private PostImage(
        PostImageId id,
        string storedFileName,
        string originalFileName,
        string? contentType,
        long fileSize)
        : base(id, storedFileName, originalFileName, contentType, fileSize)
    {

    }

    public static PostImage Create(
        string storedFileName,
        string originalFileName,
        string? contentType,
        long fileSize
        )
    {
        return new PostImage(
            PostImageId.CreateUnique(),
            storedFileName,
            originalFileName,
            contentType,
            fileSize);
    }
    
    private PostImage() { }
}