using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Repository
{
    public class UserRepo : IUserRepo
    {
        private AppDbContext _dbContext;
        public UserRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            return await _dbContext.USER
                .Where(u => u.Id == id)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName ?? "",
                    Email = u.Email
                })
                .FirstOrDefaultAsync() ?? throw new Exception("User not found");
        }

        public async Task<UserResponse> GetUserByEmail(string email)
        {
            return await _dbContext.USER
                .Where(u => u.Email == email)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName ?? "",
                    Email = u.Email
                })
                .FirstOrDefaultAsync() ?? throw new Exception("User not found");
        }
    }
}