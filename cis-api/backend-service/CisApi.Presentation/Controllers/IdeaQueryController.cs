using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CisApi.Presentation.Dtos;
using CisApi.Core.Domain.Services;

namespace CisApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/topics/{topicId}/ideas")]
    public class IdeaQueryController : ControllerBase
    {
        private readonly IIdeaQueryService _service;

        public IdeaQueryController(IIdeaQueryService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Gets all ideas for a specific topic.
        /// </summary>
        /// <param name="topicId">The topic ID</param>
        /// <returns>List of ideas. Empty array if no ideas found.</returns>
        /// <response code="200">Returns the list of ideas (can be empty)</response>
        [HttpGet]
        [ProducesResponseType(typeof(IdeaResponseDto[]), 200)]
        public async Task<ActionResult<IdeaResponseDto[]>> GetIdeasAsync([FromRoute] int topicId)
        {
            var ideas = await _service.GetIdeasByTopicIdAsync(topicId);

            var response = ideas.Select(idea => new IdeaResponseDto
            {
                Id = idea.Id,
                TopicId = idea.TopicId,
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
            }).ToArray();

            return Ok(response);
        }
    }
}