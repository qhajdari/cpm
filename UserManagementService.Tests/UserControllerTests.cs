using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using UserManagementService.Controllers;
using UserManagementService.Data;
using UserManagementService.Models;
using UserManagementService.DTO;
using System.Collections.Generic;

namespace UserManagementService.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly UserContext _context;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new UserContext(options);
            _loggerMock = new Mock<ILogger<UserController>>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new UserController(_context, _loggerMock.Object, _configurationMock.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Users.Add(new User
            {
                Id = 1,
                Email = "testuser@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password"),
                FirstName = "Test",
                LastName = "User"
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task Register_ShouldRegisterNewUser()
        {
            // Arrange
            var newUser = new User
            {
                Email = "newuser@example.com",
                Password = "newpassword",
                FirstName = "New",
                LastName = "User"
            };

            // Act
            var result = await _controller.Register(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var registeredUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "newuser@example.com");
            Assert.NotNull(registeredUser);
        }

        [Fact]
        public async Task Login_ShouldReturnTokenIfCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "testuser@example.com",
                Password = "password"
            };

            _configurationMock.SetupGet(x => x["Jwt:Key"]).Returns("sada123232asdasdads2312312jjgjhg67576jhjkg//jhggfd@gfhhj");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = ((dynamic)okResult.Value).Token;
            Assert.NotNull(token);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorizedIfCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "testuser@example.com",
                Password = "wrongpassword"
            };

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Me_ShouldReturnUserDetailsIfAuthorized()
        {
            // Arrange
            var user = new User
            {
                Id = 100,
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            // Seed the database with the test user
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Mock the HttpContext to simulate an authorized user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Me();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as IDictionary<string, object>;
            Assert.NotNull(value);

            // Ensure 'Email' key exists and verify its value
            Assert.True(value.ContainsKey("Email"));
            Assert.Equal(user.Email, value["Email"].ToString());

            // Ensure 'FirstName' key exists and verify its value
            Assert.True(value.ContainsKey("FirstName"));
            Assert.Equal(user.FirstName, value["FirstName"].ToString());

            // Ensure 'LastName' key exists and verify its value
            Assert.True(value.ContainsKey("LastName"));
            Assert.Equal(user.LastName, value["LastName"].ToString());
        }

        [Fact]
        public async Task Me_ShouldReturnUnauthorizedIfNotAuthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() // No user set in the context
            };

            // Act
            var result = await _controller.Me();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
