namespace JobHuntOS.Application.DTOs.Note;

public class NoteResponseDto
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}