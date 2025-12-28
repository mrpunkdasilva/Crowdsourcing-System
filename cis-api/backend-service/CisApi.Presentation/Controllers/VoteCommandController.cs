using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using System.Threading.Tasks;
using CisApi.Presentation.Dtos;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Services;

namespace CisApi.Presentation.Controllers;

[ApiController]
[Route("api/v1/topics/{topicId}/ideas/{ideaId}")]
public class VoteCommandController : ControllerBase
{
    private readonly IVoteCommandService _voteService;
    private readonly IMapper _mapper;

    public VoteCommandController(IVoteCommandService voteService, IMapper mapper)
    {
        _voteService = voteService;
        _mapper = mapper;
    }

    [HttpPost("vote")]
    public async Task<ActionResult<IdeaResponseDto>> CreateVoteAsync(
        [FromRoute] int topicId,
        [FromRoute] int ideaId
        )
    {
        if (HttpContext.Items["User"] is not User user)
            return Unauthorized("User information not found");

        var idea = await _voteService.CreateVoteAsync(user, ideaId);
        var response = _mapper.Map<IdeaResponseDto>(idea);

        return Ok(response);
    }

    [HttpPost("unvote")]
    public async Task<ActionResult<IdeaResponseDto>> DeleteVoteAsync(
        [FromRoute] int topicId,
        [FromRoute] int ideaId
        )
    {
        if (HttpContext.Items["User"] is not User user)
            return Unauthorized("User information not found");

        var idea = await _voteService.DeleteVoteAsync(user, ideaId);
        var response = _mapper.Map<IdeaResponseDto>(idea);

        return Ok(response);
    }
 
}
