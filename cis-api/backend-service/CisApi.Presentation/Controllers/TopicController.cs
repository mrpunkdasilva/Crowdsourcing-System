// <copyright file="TopicCommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CisApi.Presentation.Dtos;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Services;

namespace CisApi.Presentation.Controllers;

/// <summary>
/// Controller for handling commands related to topics.
/// </summary>
[ApiController]
[Route("api/v1/topics")]
// [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
public class TopicController : ControllerBase
{
    private readonly ITopicCommandService _service;

    public TopicController(ITopicCommandService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost]
    public async Task<ActionResult<TopicResponseDto>> CreateTopicAsync(TopicRequestDto request)
    {
        if (HttpContext.Items["User"] is not User user)
        {
            return Unauthorized("User information not found");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var topic = new Topic
        {
            Title = request.Title,
            Description = request.Description
        };

        try
        {

            var result = await _service.CreateTopicAsync(user, topic);

            var response = new TopicResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                CreatedAt = result.CreatedAt,
                CreatedBy = new UserDto 
            { 
                    Id = result.CreatedBy.Id,
                    Login = result.CreatedBy.Login ?? ""
            },
                Ideas = new List<IdeaDto>() 
            };
            return CreatedAtAction(nameof(GetTopicById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<TopicResponseDto> GetTopicById(int id)
    {
        return NotFound(); // Placeholder implementation
    }
}
