﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_Project.Dto;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;

namespace PRN231_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Project_PRN231Context _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public AuthController(IMapper mapper, IUserRepository userRepository, Project_PRN231Context context)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _context = context;
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

            return Ok("Login successful.");
        }
    }

}
