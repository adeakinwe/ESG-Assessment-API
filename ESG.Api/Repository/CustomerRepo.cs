using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Models;
using ESG.API.DTOs;

namespace ESG.Api.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;

        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }
        public bool CreateCustomer(CustomerForCreationDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var newCustomer = new CUSTOMER
            {
                CUSTOMERCODE = model.customerCode,
                FIRSTNAME = model.firstName,
                LASTNAME = model.lastName,
                GENDER = model.gender,
                SECTOR = model.sector,
                ADDRESS = model.address
            };

            _context.CUSTOMER.Add(newCustomer);
            return SaveChanges();
        }

        public List<CustomerForReturnDTO> GetAllCustomers()
        {
            var customers = _context.CUSTOMER.Select(x => new CustomerForReturnDTO
            {
                customerCode = x.CUSTOMERCODE,
                customerName = x.FIRSTNAME + " " + x.LASTNAME,
                gender = x.GENDER,
                sectorName = x.SECTOR == 1 ? "Agriculture" : x.SECTOR == 2 ? "Oil and Gas" : "Others",
                address = x.ADDRESS
            }).ToList();

            return customers;
        }

        public CustomerForReturnDTO GetCustomerById(int id)
        {
            var customer = _context.CUSTOMER.Where(x => x.CUSTOMERID == id).Select(x => new CustomerForReturnDTO
            {
                customerCode = x.CUSTOMERCODE,
                customerName = x.FIRSTNAME + " " + x.LASTNAME,
                gender = x.GENDER,
                sectorName = x.SECTOR == 1 ? "Agriculture" : x.SECTOR == 2 ? "Oil and Gas" : "Others",
                address = x.ADDRESS
            }).FirstOrDefault();

            return customer ?? new CustomerForReturnDTO();
        }

        public IEnumerable<CustomerForReturnDTO> SearchCustomers(string param)
        {
            if (string.IsNullOrWhiteSpace(param) || param.Length < 4)
                return new List<CustomerForReturnDTO>();

            return _context.CUSTOMER
                .Where(c => c.CUSTOMERCODE.Contains(param) ||
                            c.FIRSTNAME.Contains(param) ||
                            c.LASTNAME.Contains(param))
                .Select(c => new CustomerForReturnDTO
                {
                    customerId = c.CUSTOMERID,
                    customerCode = c.CUSTOMERCODE,
                    customerName = c.FIRSTNAME + " " + c.LASTNAME,
                    sectorId = c.SECTOR,
                    sectorName = c.SECTOR == 1 ? "Agriculture" : c.SECTOR == 2 ? "Oil and Gas" : "Others"
                })
                .OrderBy(c => c.customerName)
                .ToList();
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}