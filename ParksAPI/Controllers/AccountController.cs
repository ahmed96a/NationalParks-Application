using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ParksAPI.Models.Repository.IRepository;
using ParksModels.Dtos;
using ParksModels.Models;

namespace ParksAPI.Controllers
{
    // 12. Part 8
    // --------------------

    [Authorize]
    [Route("api/v{version:apiVersion}/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // 12. Part 8
        // --------------------

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Authenticate([FromBody] LoginDto model)
        {
            var userDto = _userRepository.Authenticate(model.UserName, model.Password);

            if(userDto == null)
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }

            return Ok(userDto);
        }

        // --------------------

        // 12. Part 8
        // --------------------

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            var isUnique = _userRepository.IsUserUnique(model.UserName);

            if(isUnique)
            {
                var userDto = _userRepository.Register(model);

                if(userDto == null)
                {
                    return BadRequest(new { message = "Error while registering." });
                }

                return Ok();
            }
            return BadRequest(new { message = "UserName is already used" });
        }

        // --------------------


    }

    // --------------------
}
