using System;

using Contracts;

using MassTransit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DoSomethingController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DoSomething(double lhs, double rhs, [FromServices] IBus bus, CancellationToken cancellationToken)
    {
        await bus.Publish(new DoSomething(lhs, rhs), cancellationToken);

        return NoContent();
    }
}
