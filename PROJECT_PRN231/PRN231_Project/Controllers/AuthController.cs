using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PRN231_Project.Dto;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Project_PRN231Context _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private AppSettings appSettings;
        public AuthController(IMapper mapper, IUserRepository userRepository, Project_PRN231Context context, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _context = context;
            appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == userDto.Username);

            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var userMap = _mapper.Map<User>(userDto);
            if (!_userRepository.CreateUser(userMap))
                return BadRequest(ModelState);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userDto.Username);

            if (user == null || !userDto.Password.Equals(user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(GenerateToken(user));
        }

        private string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("UserName", user.Username),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Password", user.Password),
                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            return accessToken;
        }
    }

}
