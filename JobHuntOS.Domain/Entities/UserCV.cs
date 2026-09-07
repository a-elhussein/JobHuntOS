namespace JobHuntOS.Domain.Entities;

public class UserCV
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? FileName { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}