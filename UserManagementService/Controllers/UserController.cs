using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models;
using UserManagementService.Data;

namespace UserManagementService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserContext _context;

    public UserController(UserContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        // Add authentication logic here
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        // Add logic to return current user details
        return Ok(User.Identity.Name);
    }
}

