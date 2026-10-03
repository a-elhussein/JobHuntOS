namespace JobHuntOS.Application.DTOs.Cv;

public class CvResponseDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? FileName { get; set; }
    public DateTime UploadedAt { get; set; }
}