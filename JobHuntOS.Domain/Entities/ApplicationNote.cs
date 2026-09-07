namespace JobHuntOS.Domain.Entities;

public class ApplicationNote
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; } =   DateTime.Now;

    public JobApplication? Application { get; set; } = null;
}