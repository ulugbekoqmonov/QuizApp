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
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(GetAllUsers));
        }
        public ActionResult Login()
        {
            return View("CreateUser");
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                User mappedUser = _mapper.Map<User>(userDto);
                User user = await _userRepository.CreateAsync(mappedUser);
                return RedirectToAction(nameof(GetAllUsers));
            }
            else
            {
                return View(userDto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllAsync();
            List<GetAllUsersDto> mappedUsers = _mapper.Map<List<GetAllUsersDto>>(users);
            ViewData["users"] = mappedUsers;
            return View(mappedUsers);
        }   
        [HttpGet]
        public async Task<IActionResult> GetByIdUser(Guid userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            return View(user);
        }

        public ActionResult EditUser()
        {
            return View("UpdateUser", new UpdateUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                User mappedUser = _mapper.Map<User>(dto);
                User user = await _userRepository.UpdateAsync(mappedUser);
            }
            return RedirectToAction(nameof(GetAllUsers));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _userRepository.DeleteAsync(userId);
            return RedirectToAction(nameof(GetAllUsers));
        }
    }
}
