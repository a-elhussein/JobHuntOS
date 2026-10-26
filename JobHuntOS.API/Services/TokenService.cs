using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobHuntOS.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;

namespace JobHuntOS.API.Services;

public class TokenService: ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public (string Token, DateTime ExpiresAt) CreateToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var expires = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}