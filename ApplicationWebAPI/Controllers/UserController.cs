using Application.Interfaces;
using Application.Models.DTOs.User;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ApiControllerBase<User>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly double _refreshTokenLifeTime;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserRepository accountRepository, IJwtTokenService jwtTokenService, IAuthenticationService authenticationService, IUserTokenRepository userTokenRepository, IConfiguration configuration, ILogger<UserController> logger)
    {
        _userRepository = accountRepository;
        _jwtTokenService = jwtTokenService;
        _authenticationService = authenticationService;
        _userTokenRepository = userTokenRepository;
        _refreshTokenLifeTime = double.Parse(configuration["JWT:RefreshTokenLifeTime"]);
        _logger = logger;
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<IQueryable<UserGetDto>>>> GetAllUsers()
    {
        Task<IQueryable<User>> Users = _userRepository.GetAllAsync();

        IQueryable<UserGetDto> mappedUsers = _mapper.Map<IQueryable<UserGetDto>>(Users);

        return Ok(new Response<IQueryable<UserGetDto>>(mappedUsers));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<UserGetDto>>> GetByIdUser(Guid id)
    {
        User? user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new Response<User?>(false, id + " not found!"));
        }
        UserGetDto mappedUser = _mapper.Map<UserGetDto>(user);
        return Ok(new Response<UserGetDto?>(mappedUser));
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<UserGetDto>>> UpdateUser([FromBody] UserUpdateDto User)
    {
        User? mappedUser = _mapper.Map<User>(User);
        var validationResult = _validator.Validate(mappedUser);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<User>(false, validationResult.Errors));
        }
        User user = await _userRepository.UpdateAsync(mappedUser);
        return Ok(new Response<UserGetDto>(_mapper.Map<UserGetDto>(user)));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<Response<UserGetDto>>> CreateUser([FromBody] UserCreateDto User)
    {
        User mappedUser = _mapper.Map<User>(User);
        var validationResult = _validator.Validate(mappedUser);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<UserCreateDto>(false, validationResult.Errors));
        }
        User user = await _userRepository.CreateAsync(mappedUser);
        UserGetDto result = _mapper.Map<UserGetDto>(user);
        return Ok(new Response<UserGetDto>(result));
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult<Response<bool>>> Delete(Guid id)
    {
        return await _userRepository.DeleteAsync(id) ?
            Ok(new Response<bool>(true))
          : BadRequest(new Response<bool>(false, "Delete failed!"));
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<Response<PaginatedList<UserGetDto>>>> Search(string text, int page = 1, int pageSize = 10)
    {
        var users = _userRepository.GetAllAsync(u=>u.UserName.Contains(text)||
                                                            u.Email.Contains(text)||
                                                            u.Phone.Contains(text));
        var mappedUsers = _mapper.Map<IQueryable<UserGetDto>>(users);
        PaginatedList<UserGetDto> paginatedUsers = new (mappedUsers, page, pageSize);

        Response<PaginatedList<UserGetDto>> result = new()
        {
            Result = paginatedUsers
        };
        return Ok(result);
    }
}