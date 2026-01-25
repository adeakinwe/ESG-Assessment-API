using ESG.Api.Interface;
using ESG.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ESG.Api.Controller
{
    [EnableRateLimiting(RateLimitPolicies.Sensitive)]
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _repo;

        public CustomerController(ICustomerRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("all")]
        [EnableRateLimiting(RateLimitPolicies.PublicRead)]
        public ActionResult<List<CustomerForReturnDTO>> GetAllCustomers()
        {
            var customers = _repo.GetAllCustomers();

            if (customers != null)
            {
                return Ok(customers);
            }

            return NotFound();
        }

        [HttpGet("{id:int}")]
        [EnableRateLimiting(RateLimitPolicies.PublicRead)]
        public ActionResult<CustomerForReturnDTO> GetCustomerByCustomerId(int id)
        {
            var customer = _repo.GetCustomerById(id);

            if (customer != null)
            {
                return Ok(customer);
            }

            return NotFound();
        }

        [HttpPost("add")]
        [EnableRateLimiting(RateLimitPolicies.WriteHeavy)]
        public ActionResult<CustomerForReturnDTO> CreateCustomer(CustomerForCreationDTO newCustomer)
        {
            ArgumentNullException.ThrowIfNull(newCustomer);

            bool status = _repo.CreateCustomer(newCustomer);

            if (status)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpGet("search")]
        [EnableRateLimiting(RateLimitPolicies.PublicRead)]
        public ActionResult<IEnumerable<CustomerForReturnDTO>> GetCustomers([FromQuery] string param)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(param);
                var customers = _repo.SearchCustomers(param);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request: " + ex.Message);
            }
        }
    }
}