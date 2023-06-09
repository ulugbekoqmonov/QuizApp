using Application.Interfaces;
using Application.Models.DTOs.Role;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ApiControllerBase<Role>
{
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<IQueryable<RoleGetDto>>>> GetAllRoles()
    {
        Task<IQueryable<Role>> Roles = _roleRepository.GetAllAsync();

        IQueryable<RoleGetDto> mappedRoles = _mapper.Map<IQueryable<RoleGetDto>>(Roles);

        return Ok(new Response<IQueryable<RoleGetDto>>(mappedRoles));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<RoleGetDto>>> GetByIdRole(Guid id)
    {
        Role? Role = await _roleRepository.GetByIdAsync(id);
        if (Role == null)
        {
            return NotFound(new Response<Role?>(false, id + " not found!"));
        }
        RoleGetDto mappedRole = _mapper.Map<RoleGetDto>(Role);
        return Ok(new Response<RoleGetDto?>(mappedRole));
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<RoleGetDto>>> UpdateRole([FromBody] RoleUpdateDto Role)
    {
        Role? mappedRole = _mapper.Map<Role>(Role);
        var validationResult = _validator.Validate(mappedRole);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<Role>(false, validationResult.Errors));
        }
        Role role = await _roleRepository.UpdateAsync(mappedRole);
        return Ok(new Response<RoleGetDto>(_mapper.Map<RoleGetDto>(role)));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<Response<RoleGetDto>>> CreateRole([FromBody] RoleCreateDto Role)
    {
        Role mappedRole = _mapper.Map<Role>(Role);
        var validationResult = _validator.Validate(mappedRole);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<RoleCreateDto>(false, validationResult.Errors));
        }
        Role role = await _roleRepository.CreateAsync(mappedRole);
        RoleGetDto result = _mapper.Map<RoleGetDto>(role);
        return Ok(new Response<RoleGetDto>(result));
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult<Response<bool>>> Delete(Guid id)
    {
        return await _roleRepository.DeleteAsync(id) ?
            Ok(new Response<bool>(true))
          : BadRequest(new Response<bool>(false, "Delete failed!"));
    }
}
