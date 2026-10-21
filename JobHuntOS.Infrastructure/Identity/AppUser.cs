using Microsoft.AspNetCore.Identity;

namespace JobHuntOS.Infrastructure.Identity;

public class AppUser: IdentityUser<Guid>
{
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}