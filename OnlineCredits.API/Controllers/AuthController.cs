using Microsoft.AspNetCore.Mvc;
using OnlineCredits.Core.DTOs.Auth;
using OnlineCredits.Core.Entities;
using OnlineCredits.Infrastructure.Data;
using OnlineCredits.Infrastructure.Services;

namespace OnlineCredits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, PasswordHasher passwordHasher, JwtService jwtService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return BadRequest("El nombre de usuario ya está en uso.");
            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("El email ya está en uso.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DocumentType = dto.DocumentType,
                DocumentNumber = dto.DocumentNumber,
                PhoneNumber = dto.PhoneNumber,
                Role = "Solicitante",
                Status = "Activo",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuario registrado exitosamente." });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto dto)
        {
            // Buscar usuario por username o email
            var user = _context.Users.FirstOrDefault(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

            if (user == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

            // Verificar contraseña
            if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

            // Generar token JWT
            var token = _jwtService.GenerateToken(user);

            return Ok(new { message = "Login exitoso.", token });
        }
    }
} 