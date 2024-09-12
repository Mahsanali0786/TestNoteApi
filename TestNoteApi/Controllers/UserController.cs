namespace SampleProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SampleProject.DbContext;
    using SampleProject.Models.Dto;
    using SampleProject.Models;
    using System.Security.Cryptography;
    using System.Text;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SampleDbContext _context;

        public UserController(SampleDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid input");

            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null) return Conflict("Email already in use");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("User registered successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Registration failed");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid input");

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            if (!VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            return Ok("Login successful");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }

}
