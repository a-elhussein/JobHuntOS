using JobHuntOS.API.Services;
using JobHuntOS.Application.DTOs.Auth;
using JobHuntOS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntOS.API.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<AppUser>  userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDtos.RegisterDto dto)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.Email,
            Email = dto.Email,
            DisplayName = dto.DisplayName,
        };
        
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));
        return Ok(BuildResponse(user));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDtos.LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        // Same message for "no such user" and "wrong password"
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(BuildResponse(user));
    }
    
    
    private AuthDtos.AuthResponseDto BuildResponse(AppUser user)
    {
        var (token, expires) = _tokenService.CreateToken(user);
        return new AuthDtos.AuthResponseDto { Token = token, Email = user.Email!, ExpiresAt = expires };
    }
}