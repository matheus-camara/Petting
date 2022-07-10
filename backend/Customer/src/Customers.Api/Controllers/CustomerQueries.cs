using Customers.Api.QueryParameters;
using Customers.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers
{
    [Route("Customer")]
    [ApiController]
    public class CustomerQueries : ControllerBase
    {
        private ICustomerRepository Repository { get; }

        public CustomerQueries(ICustomerRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> FindAsync([FromRoute] Guid id)
        {
            return Ok(await Repository.FindAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int? page,
            [FromQuery] int? size)
        {
            var request = new PageRequest(page, size);
            return Ok(await Repository.GetAsync(request.Skip, request.Take));
        }
    }
}
