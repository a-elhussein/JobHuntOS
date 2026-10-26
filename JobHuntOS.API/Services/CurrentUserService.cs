using System.Security.Claims;
using JobHuntOS.Application.Interfaces;

namespace JobHuntOS.API.Services;

public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
            
            return Guid.TryParse(value, out var id) 
                ? id : throw new UnauthorizedAccessException("No authenticated user.");
        }
    }
}