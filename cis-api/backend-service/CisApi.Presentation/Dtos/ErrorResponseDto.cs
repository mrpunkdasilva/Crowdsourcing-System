// <copyright file="ErrorResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for a generic error response.
/// </summary>
public class ErrorResponseDto
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public required string Message { get; set; }
}
