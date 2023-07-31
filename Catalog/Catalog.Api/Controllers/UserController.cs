using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Catalog.Api.Models;
using Catalog.Core.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.Api.Controllers;


[ApiController]
[Route("/api/[controller]")]
public class UserController : Controller
{
    private readonly UserManager<CatalogUser> _userManager;
    private readonly SignInManager<CatalogUser> _signInManager;

    public UserController(UserManager<CatalogUser> userManager, SignInManager<CatalogUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var user = new CatalogUser
        {
            UserName = registerModel.Email,
            Email = registerModel.Email,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return Unauthorized();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                Token = token
            });
        }

        return Unauthorized();
    }
}