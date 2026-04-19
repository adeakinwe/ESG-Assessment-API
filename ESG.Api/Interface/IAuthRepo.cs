using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IAuthRepo
    {
        Task<AuthResponseDTO> Register(AuthRequestDTO request);
        Task<AuthResponseDTO> Login(LoginRequestDTO request);
    }
}