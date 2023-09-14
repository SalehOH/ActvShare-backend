namespace ActvShare.Domain.Common.Models;

public abstract class ImageEntity<TId>: Entity<TId> where TId: ValueObject
{
    public string StoredFileName { get; init; }
    public string OriginalFileName { get; init; }
    public string? ContentType { get; init; }
    public long FileSize { get; init; }
    public DateTime UploadedAt { get; init; }

    protected ImageEntity(
        TId tId, 
        string storedFileName, 
        string originalFileName, 
        string? contentType, 
        long fileSize):base(tId)
    {
        StoredFileName = storedFileName;
        OriginalFileName = originalFileName;
        ContentType = contentType;
        FileSize = fileSize;
        UploadedAt = DateTime.UtcNow;
    }
    
    protected ImageEntity(){}
}