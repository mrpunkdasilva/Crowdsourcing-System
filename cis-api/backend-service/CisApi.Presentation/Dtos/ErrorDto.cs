// <copyright file="ErrorDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for an error response.
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets a machine-readable error code.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Gets or sets a short, human-readable summary of the problem.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets a human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    public required string Detail { get; set; }
}
