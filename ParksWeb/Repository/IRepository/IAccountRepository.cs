using ParksModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksWeb.Repository.IRepository
{
    // 13. Part 1
    // ---------------------------

    public interface IAccountRepository
    {
        Task<UserDto> LoginAsync(string url, LoginDto loginDto, string token);

        Task<bool> RegisterAsync(string url, RegisterDto registerDto, string token);
    }

    // ---------------------------
}
