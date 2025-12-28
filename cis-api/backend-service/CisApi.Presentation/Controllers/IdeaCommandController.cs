using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CisApi.Presentation.Dtos;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Services;

namespace CisApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/topics/{topicId}/ideas")]
    public class IdeaCommandController : ControllerBase
    {
        private readonly IIdeaCommandService _service;

        public IdeaCommandController(IIdeaCommandService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdeaResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IdeaResponseDto>> CreateIdeaAsync(
            [FromRoute] int topicId,
            [FromBody] IdeaRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (HttpContext.Items["User"] is not User user)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _service.CreateIdeaForTopicAsync(topicId, request.Title, request.Description, user);

                var response = new IdeaResponseDto
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    VotedBy = result.VotedBy,

                    VoteCount = result.VoteCount,
                    CreatedAt = result.CreatedAt,
                    CreatedBy = new UserDto
                    {
                        Id = result.CreatedBy.Id,
                        Login = result.CreatedBy.Login ?? ""
                    }
                };

                return CreatedAtAction(
                    nameof(GetIdeaById),
                    new { topicId = topicId, ideaId = response.Id },
                    response);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Topic with ID") && ex.Message.Contains("not found"))
                {
                    return NotFound(new ErrorResponseDto { Message = ex.Message });
                }
                if (ex.Message.Contains("User with ID") && ex.Message.Contains("not found"))
                {
                    return Unauthorized(new ErrorResponseDto { Message = ex.Message });
                }
                throw; 
            }
        }

        [HttpGet("{ideaId}")]
        [ProducesResponseType(typeof(IdeaResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IdeaResponseDto>> GetIdeaById(int topicId, int ideaId)
        {
            try
            {
                var idea = await _service.GetIdeaByIdAsync(topicId, ideaId);

                var response = new IdeaResponseDto
                {
                    Id = idea.Id,
                    Title = idea.Title,
                    Description = idea.Description,
                    VotedBy = idea.VotedBy,
                    VoteCount = idea.VoteCount,
                    CreatedAt = idea.CreatedAt,
                    CreatedBy = new UserDto
                    {
                        Id = idea.CreatedBy.Id,
                        Login = idea.CreatedBy.Login ?? ""
                    }
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new ErrorResponseDto { Message = ex.Message });
                }
                throw; // Re-throw if it's another InvalidOperationException
            }
        }
    }
}
