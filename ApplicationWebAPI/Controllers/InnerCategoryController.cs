using Application.Interfaces;
using Application.Models.DTOs.InnerCategory;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InnerCategoryController : ApiControllerBase<InnerCategory>
{
    private readonly IInnerCategoryRepository _innerCategoryRepository;
    public InnerCategoryController(IInnerCategoryRepository innerCategoryRepository)
    {
        _innerCategoryRepository = innerCategoryRepository;
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<Response<IQueryable<InnerCategoryGetDto>>>> GetAllInnerCategories()
    {
        Task<IQueryable<InnerCategory>> InnerCategorys = _innerCategoryRepository.GetAllAsync();

        IQueryable<InnerCategoryGetDto> mappedInnerCategorys = _mapper.Map<IQueryable<InnerCategoryGetDto>>(InnerCategorys);

        return Ok(new Response<IQueryable<InnerCategoryGetDto>>(mappedInnerCategorys));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<InnerCategoryGetDto>>> GetByIdInnerCategory(Guid id)
    {
        InnerCategory? InnerCategory = await _innerCategoryRepository.GetByIdAsync(id);
        if (InnerCategory == null)
        {
            return NotFound(new Response<InnerCategoryGetDto?>(false, id + " not found!"));
        }
        InnerCategoryGetDto mappedInnerCategory = _mapper.Map<InnerCategoryGetDto>(InnerCategory);
        return Ok(new Response<InnerCategoryGetDto?>(mappedInnerCategory));
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<InnerCategoryGetDto>>> UpdateInnerCategory([FromBody] InnerCategoryUpdateDto InnerCategory)
    {
        InnerCategory? mappedInnerCategory = _mapper.Map<InnerCategory>(InnerCategory);
        var validationResult = _validator.Validate(mappedInnerCategory);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<InnerCategory>(false, validationResult.Errors));
        }
        InnerCategory innerCategory = await _innerCategoryRepository.UpdateAsync(mappedInnerCategory);
        return Ok(new Response<InnerCategoryGetDto>(_mapper.Map<InnerCategoryGetDto>(innerCategory)));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<Response<InnerCategoryGetDto>>> CreateInnerCategory([FromBody] InnerCategoryCreateDto InnerCategory)
    {
        InnerCategory mappedInnerCategory = _mapper.Map<InnerCategory>(InnerCategory);
        var validationResult = _validator.Validate(mappedInnerCategory);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<InnerCategoryCreateDto>(false, validationResult.Errors));
        }
        InnerCategory innerCategory = await _innerCategoryRepository.CreateAsync(mappedInnerCategory);
        InnerCategoryGetDto result = _mapper.Map<InnerCategoryGetDto>(innerCategory);
        return Ok(new Response<InnerCategoryGetDto>(result));
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult<Response<bool>>> Delete(Guid id)
    {
        return await _innerCategoryRepository.DeleteAsync(id) ?
            Ok(new Response<bool>(true))
          : BadRequest(new Response<bool>(false, "Delete failed!"));
    }
}