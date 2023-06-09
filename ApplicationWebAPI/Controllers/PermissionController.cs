using Application.Interfaces;
using Application.Models.DTOs.Permission;
using Application.Models.DTOs.Role;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionController : ApiControllerBase<PermissionCreateDto>
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionController(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<Response<IQueryable<PermissionGetDto>>>> GetAllPermissions()
    {
        IQueryable<Permission> permissions = await _permissionRepository.GetAllAsync();
        IQueryable<PermissionGetDto> mappedPermission = _mapper.Map<IQueryable<PermissionGetDto>>(permissions);
        return Ok(new Response<IQueryable<PermissionGetDto>>(mappedPermission));
    }
    [HttpPost("[action]")]
    public async Task<ActionResult<Response<PermissionGetDto>>> CreatePermission([FromBody] PermissionCreateDto permission)
    {
        var validationResult = _validator.Validate(permission);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<PermissionGetDto>(false, validationResult.Errors));
        }
        Permission mappedPermission = _mapper.Map<Permission>(permission);
        Permission Permission = await _permissionRepository.CreateAsync(mappedPermission);
        PermissionGetDto result = _mapper.Map<PermissionGetDto>(Permission);
        return Ok(new Response<PermissionGetDto>(result));
    }
}
