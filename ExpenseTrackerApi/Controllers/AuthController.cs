using Azure.Core;
using ExpenseTrackerApi.Authentification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTrackerApi.Controllers;

[Route("api/authentification")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;

    public AuthController(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public record LoginRequestDto(string UserName, string Password);

    public record AuthResponseDto(string Token, DateTime ExpiresAtUtc, DateTime ExpiresAtBucharest);

    [HttpPost("login")]
    [AllowAnonymous]

    public ActionResult<AuthResponseDto> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        if (string.IsNullOrWhiteSpace(loginRequestDto.UserName) || string.IsNullOrWhiteSpace(loginRequestDto.Password))
            return BadRequest("Username and password are requested.");

        if (loginRequestDto.UserName != "admin" || loginRequestDto.Password != "gabita!")
            return Unauthorized("Invalid credentials.");

        var claims = new List<Claim>
        {
                new Claim(JwtRegisteredClaimNames.Sub, loginRequestDto.UserName),          
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
                new Claim(ClaimTypes.Name, loginRequestDto.UserName),                       
                new Claim(ClaimTypes.Role, "Admin")                                  
        };

        var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);                         
        var signingKey = new SymmetricSecurityKey(keyBytes);                     
        var signingCreds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var nowUtc = DateTime.UtcNow;
        var expiresUtc = nowUtc.AddMinutes(_jwtSettings.ExpirationMinutes);               
        var notBeforeUtc = nowUtc.AddSeconds(-5);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,                                                
            audience: _jwtSettings.Audience,                                            
            claims: claims,                                                     
            notBefore: notBeforeUtc,                                            
            expires: expiresUtc,                                               
            signingCredentials: signingCreds                                    
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        Console.WriteLine($"Issued JWT: {tokenString}");
        var roTz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");
        var expiresRo = TimeZoneInfo.ConvertTimeFromUtc(expiresUtc, roTz);

        return Ok(new AuthResponseDto(tokenString, expiresUtc, expiresRo));
    }
}
