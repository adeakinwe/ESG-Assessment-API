using System.Security.Cryptography;
using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using ESG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDbContext _dbContext;
        public AuthRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthResponseDTO> Login(AuthRequestDTO request)
        {
            var user = await _dbContext.USER.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new Exception("User not found");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid password");

            var userResponse = new AuthResponseDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return userResponse;
        }

        public async Task<AuthResponseDTO> Register(AuthRequestDTO request)
        {
            if (await _dbContext.USER.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Email is already registered.");

            CreatePasswordHash(request.Password, out var hash, out var salt);

            var user = new USER
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _dbContext.USER.Add(user);
            await _dbContext.SaveChangesAsync();

            var userResponse = new AuthResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return userResponse;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}