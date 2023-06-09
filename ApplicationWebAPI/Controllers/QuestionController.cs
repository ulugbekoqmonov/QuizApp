using Application.Interfaces;
using Application.Models.DTOs.Question;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionController : ApiControllerBase<Question>
{
    private readonly IQuestionRepository _questionRepository;
    public QuestionController(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<IQueryable<QuestionGetDto>>>> GetAllQuestions()
    {
        Task<IQueryable<Question>> Questions = _questionRepository.GetAllAsync();

        IQueryable<QuestionGetDto> mappedQuestions = _mapper.Map<IQueryable<QuestionGetDto>>(Questions);

        return Ok(new Response<IQueryable<QuestionGetDto>>(mappedQuestions));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<Response<QuestionGetDto>>> GetByIdQuestion(Guid id)
    {
        Question? question = await _questionRepository.GetByIdAsync(id);
        if (question == null)
        {
            return NotFound(new Response<QuestionGetDto?>(false, id + " not found!"));
        }
        QuestionGetDto mappedQuestion = _mapper.Map<QuestionGetDto>(question);
        return Ok(new Response<QuestionGetDto?>(mappedQuestion));
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<QuestionGetDto>>> UpdateQuestion([FromBody] QuestionUpdateDto question)
    {
        Question? mappedQuestion = _mapper.Map<Question>(question);
        var validationResult = _validator.Validate(mappedQuestion);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<Question>(false, validationResult.Errors));
        }
        Question Question = await _questionRepository.UpdateAsync(mappedQuestion);
        return Ok(new Response<QuestionGetDto>(_mapper.Map<QuestionGetDto>(Question)));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<Response<QuestionGetDto>>> CreateQuestion([FromBody] QuestionCreateDto question)
    {
        Question mappedQuestion = _mapper.Map<Question>(question);
        var validationResult = _validator.Validate(mappedQuestion);
        if (!validationResult.IsValid)
        {
            return BadRequest(new Response<QuestionCreateDto>(false, validationResult.Errors));
        }
        Question Question = await _questionRepository.CreateAsync(mappedQuestion);
        QuestionGetDto result = _mapper.Map<QuestionGetDto>(Question);
        return Ok(new Response<QuestionGetDto>(result));
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult<Response<bool>>> Delete(Guid id)
    {
        return await _questionRepository.DeleteAsync(id) ?
            Ok(new Response<bool>(true))
          : BadRequest(new Response<bool>(false, "Delete failed!"));
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<Response<PaginatedList<QuestionGetDto>>>> Search(string text, int page = 1, int pageSize = 10)
    {
        var questions = await _questionRepository.GetAllAsync(q => q.QuestionText.Contains(text) ||
                                                            q.InnerCategory.InnerCategoryName.Contains(text));
        var Questions = _mapper.Map<IQueryable<QuestionGetDto>>(questions);
        PaginatedList<QuestionGetDto> paginatedQuestions = new PaginatedList<QuestionGetDto>(Questions, page, pageSize);

        Response<PaginatedList<QuestionGetDto>> result = new()
        {
            Result = paginatedQuestions
        };
        return Ok(result);
    }
}