// <copyright file="HealthCheckController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace CisApi.Presentation.Controllers;

/// <summary>
/// Controller responsible for checking the API's health.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Returns the API's health status.
    /// </summary>
    /// <returns>An HTTP 200 OK result with the message "API is healthy!".</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok("API is healthy!");
    }
}