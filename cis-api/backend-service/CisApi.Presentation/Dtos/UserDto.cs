// <copyright file="UserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CisApi.Presentation.Dtos;

/// <summary>
/// DTO for user information.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the user's ID.
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the user's login name.
    /// </summary>
    public required string Login { get; set; }
}
