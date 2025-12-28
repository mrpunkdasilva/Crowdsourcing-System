// <copyright file="TopicRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using System.ComponentModel.DataAnnotations;

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for a topic creation request.
/// </summary>
public class TopicRequestDto
{
    /// <summary>
    /// Gets or sets the title of the topic.
    /// </summary>
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the topic.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;
}
