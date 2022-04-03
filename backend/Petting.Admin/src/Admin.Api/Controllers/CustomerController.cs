using Admin.Domain.Customers.Add;
using ApiActions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : MediatorController
{
    public CustomerController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddCustomerMessage message)
    {
        await Mediator.Send(message);
        return Ok();
    }
}