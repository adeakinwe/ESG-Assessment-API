using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IUserRepo
    {
        Task<UserResponse> GetUserById(int id);
        Task<UserResponse> GetUserByEmail(string email);
    }
}