// <copyright file="TopicResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for a topic response.
/// </summary>
public class TopicResponseDto
{
    /// <summary>
    /// Gets or sets the topic ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the topic.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the topic.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of when the topic was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who created the topic.
    /// </summary>
    public required UserDto CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the list of ideas associated with the topic.
    /// </summary>
    public List<IdeaDto> Ideas { get; set; } = new();

}
