using ESG.Api.Interface;
using ESG.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Api.Controller
{
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
        public ActionResult<List<CustomerForReturnDTO>> GetAllCustomers()
        {
            var customers = _repo.GetAllCustomers();

            if (customers != null)
            {
               return Ok(customers) ; 
            }

            return NotFound();
        }

        [HttpGet("{id:int}")]
        public ActionResult<CustomerForReturnDTO> GetCustomerByCustomerId(int id)
        {
            var customer = _repo.GetCustomerById(id);

            if (customer != null)
            {
               return Ok(customer) ; 
            }

            return NotFound();
        }

        [HttpPost("add")]
        public ActionResult<CustomerForReturnDTO> CreateCustomer(CustomerForCreationDTO newCustomer)
        {
            ArgumentNullException.ThrowIfNull(newCustomer);

            bool status = _repo.CreateCustomer(newCustomer);

            if (status)
            {
               return Ok() ; 
            }

            return NotFound();
        }
    }
}