using Microsoft.AspNetCore.Mvc;
using VaultKey.Business.Models;
using VaultKey.Business.Services;
using VaultKey.Business.Services.Interfaces;
using VaultKey.Data.Entities;
using static VaultKey.Business.Models.CustomerResponses;

namespace VaultKey.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController: ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>Creates a customer and one account. Personal data is saved encrypted.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerCreatedResponse>> Create(
            [FromBody] CreateCustomerRequest request, CancellationToken ct)
        {
            var created = await _customerService.CreateAsync(request, ct);

            return CreatedAtAction(
                nameof(GetProfile),
                new { customerNumber = created.CustomerNumber },
                created);
        }

        /// <summary>Returns profile details only (no accounts). Values are decrypted on the server.</summary>
        [HttpGet("{customerNumber}/profile")]
        [ProducesResponseType(typeof(CustomerProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerProfileResponse>> GetProfile(string customerNumber, CancellationToken ct)
        {
            var profile = await _customerService.GetProfileAsync(customerNumber, ct);
            if (profile is null)
            {
                return NotFound();
            }

            Response.Headers.CacheControl = "no-store";
            return profile;
        }
    }
}
