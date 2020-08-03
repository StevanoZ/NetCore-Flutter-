using System;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenRepository
    {
        Task<string> CreateToken(AppUser user, DateTime exp);
        string GenerateReffreshToken();
    }
}