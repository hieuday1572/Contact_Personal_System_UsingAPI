using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Project.Dto;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;

namespace PRN231_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserController(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var user = _mapper.Map<UserDto>(_userRepository.GetByUsername(name));
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }

        [HttpPost]
        public IActionResult CreateUser(UserDto user)
        {
            var userMap = _mapper.Map<User>(user);
            if (!_userRepository.CreateUser(userMap))
                return BadRequest(ModelState);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateUser(UserDto user)
        {
            var userMap = _mapper.Map<User>(user);
            if (!_userRepository.UpdateUser(userMap))
                return BadRequest(ModelState);
            return Ok();
        }
    }
}
