// <copyright file="IdeaRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using System.ComponentModel.DataAnnotations;

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for an idea creation request.
/// </summary>
public class IdeaRequestDto
{
    /// <summary>
    /// Gets or sets the title of the idea.
    /// </summary>
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(255, MinimumLength = 1)]
    public required string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the idea.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    [MinLength(1)]
    public required string Description { get; set; }
}
