using Customers.Domain.Commands.Add;
using Customers.Domain.Commands.Update;
using Microsoft.AspNetCore.Mvc;
using Paramore.Brighter;
using System.Net;

namespace Customers.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IAmACommandProcessor _processor;

    public CustomerController(IAmACommandProcessor processor)
    {
        _processor = processor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AddCustomerCommand command)
    {
        await _processor.SendAsync(command);

        return StatusCode(((int)HttpStatusCode.Created));
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCustomerCommand command)
    {
        await _processor.SendAsync(command.WithId(id));

        return Ok();
    }
}