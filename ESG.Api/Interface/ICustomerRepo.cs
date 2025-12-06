using ESG.API.DTOs;

namespace ESG.Api.Interface
{
    public interface ICustomerRepo
    {
        List<CustomerForReturnDTO> GetAllCustomers();
        CustomerForReturnDTO GetCustomerById(int id);
        bool CreateCustomer(CustomerForCreationDTO model);
        bool SaveChanges();
    }
}   