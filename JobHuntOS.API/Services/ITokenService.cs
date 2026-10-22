using JobHuntOS.Infrastructure.Identity;

namespace JobHuntOS.API.Services;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(AppUser user);
}