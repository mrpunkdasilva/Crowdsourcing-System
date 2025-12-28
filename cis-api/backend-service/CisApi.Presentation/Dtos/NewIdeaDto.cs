// <copyright file="NewIdeaDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for a new idea creation.
/// </summary>
public class NewIdeaDto
{
    /// <summary>
    /// Gets or sets the title of the idea.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the idea.
    /// </summary>
    public required string Description { get; set; }
}
