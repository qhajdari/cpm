using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementService.Models;
using UserManagementService.Data;
using RabbitMQ.Client;
using System.Text.Json;
using UserManagementService.DTO;


namespace UserManagementService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;

    public UserController(UserContext context, ILogger<UserController> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        _logger.LogInformation("Registering user with email: {Email}", user.Email);

        // Check if user already exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User with email {Email} already exists.", user.Email);
            return BadRequest("User already exists.");
        }

        // Hash the password before saving (you should add proper hashing here)
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Publish event to RabbitMQ
        //PublishUserRegisteredEvent(user);

        _logger.LogInformation("User with email {Email} registered successfully.", user.Email);
        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
    {
        _logger.LogInformation("Attempting to log in user with email: {Email}", loginRequest.Email);

        // Authenticate the user
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            _logger.LogWarning("Invalid login attempt for email: {Email}", loginRequest.Email);
            return Unauthorized("Invalid credentials.");
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        _logger.LogInformation("User with email {Email} logged in successfully.", loginRequest.Email);
        return Ok(new { Token = tokenString });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        // Get the current user ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt.");
            return Unauthorized();
        }

        _logger.LogInformation("Fetching details for user ID: {UserId}", userId);

        // Retrieve user details from the database
        var user = await _context.Users.FindAsync(int.Parse(userId));
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", userId);
            return NotFound("User not found.");
        }

        _logger.LogInformation("Details fetched for user ID: {UserId}", userId);
        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName
        });
    }

    private void PublishUserRegisteredEvent(User user)
    {
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:HostName"] };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
            channel.QueueDeclare(queue: "user_registered",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = JsonSerializer.Serialize(user);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "user_registered",
                                 basicProperties: null,
                                 body: body);

            _logger.LogInformation("Published user registered event for user with email: {Email}", user.Email);
        
    }
}
