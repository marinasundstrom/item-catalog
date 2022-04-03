﻿
using Catalog.ApiKeys.Application;
using Catalog.ApiKeys.Application.Commands;
using Catalog.ApiKeys.Application.Common.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.ApiKeys.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
//[Authorize]
public class ApiKeysController : Controller
{
    private readonly IMediator _mediator;

    public ApiKeysController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Check")]
    [ProducesResponseType(typeof(ApiKeyResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiKeyResult>> CheckApiKey(CheckApiKeyRequest request, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new CheckApiKeyCommand(request.ApiKey, request.RequestedResources), cancellationToken));
    }
}

public record CheckApiKeyRequest(string ApiKey, string[] RequestedResources);