// <copyright file="IdeaDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for an idea.
/// </summary>
public class IdeaDto
{
    /// <summary>
    /// Gets or sets the idea ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the topic ID associated with the idea.
    /// </summary>
    public int TopicId { get; set; }

    /// <summary>
    /// Gets or sets the title of the idea.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the idea.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the number of votes the idea has received.
    /// </summary>
    public int VoteCount { get; set; }

    /// <summary>
    /// Gets or sets the list of emails of users who voted for the idea.
    /// </summary>
    public required List<string> VotedBy { get; set; }
}
