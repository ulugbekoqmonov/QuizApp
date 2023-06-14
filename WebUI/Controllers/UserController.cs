using Application.DataTransferObjects.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{    
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("CreateUser");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto userDto)
        {
            User mappedUser = new()
            {
                UserName=userDto.UserName,
                Password=userDto.Password,
                Email=userDto.Email,
                Phone=userDto.PhoneNumber
            };
            User user = await _userRepository.CreateAsync(mappedUser);
            return View();
        }
    }
}
