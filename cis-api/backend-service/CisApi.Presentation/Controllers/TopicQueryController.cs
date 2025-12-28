
using AutoMapper;
using CisApi.Core.Domain.Services;
using CisApi.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CisApi.Presentation.Controllers;

/// <summary>
/// Controller responsible for handling read-only operations for topics.
/// </summary>
[ApiController]
[Route("api/v1/topics")]
public class TopicQueryController : ControllerBase
{
    private readonly ITopicQueryService _service;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicQueryController"/> class.
    /// </summary>
    /// <param name="service">The topic query service.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public TopicQueryController(ITopicQueryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all topics.
    /// </summary>
    /// <returns>An HTTP 200 OK response containing a list of topics.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TopicResponseDto>>> GetTopicsAsync()
    {
        var topics = await _service.GetTopicsAsync();
        var topicDtos = _mapper.Map<IEnumerable<TopicResponseDto>>(topics);
        return Ok(topicDtos);
    }
}
