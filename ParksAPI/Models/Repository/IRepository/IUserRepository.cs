using ParksModels.Dtos;
using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.Repository.IRepository
{
    // 12. Part 4
    // ------------------------------

    public interface IUserRepository
    {
        bool IsUserUnique(string username);

        UserDto Authenticate(string username, string password);

        UserDto Register(RegisterDto registerDto);
    }

    // ------------------------------
}
