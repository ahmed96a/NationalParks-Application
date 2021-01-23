using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParksAPI.Models.Repository.IRepository;
using ParksModels.Dtos;
using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParksAPI.Models.Repository
{
    // 12. Part 4
    // ------------------------------
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly AppSettings _appSettings; // 12. Part 7
        private readonly IMapper _mapper; // 12. Part 9

        public UserRepository(AppDbContext appDbContext, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _appSettings = appSettings.Value; // 12. Part 7
            _mapper = mapper; // 12. Part 9
        }

        // 12. Part 7
        // ---------------------

        public UserDto Authenticate(string username, string password)
        {
            // if a user is authenticated we want to create a new token and pass it back to the API call, else return null.

            var user = _appDbContext.Users.SingleOrDefault(user => user.UserName == username && user.Password == password);

            if(user == null)
            {
                return null;
            }

            // if user is validated, then we will generate a JWT token.

            // To Generate a JWT token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor() // tokenDescriptor will have all the details regarding tokens.
            {
                Subject = new ClaimsIdentity(new Claim[] { // Thus, the Claims that we set to Subject property of tokenDecriptor, will be added to the token Payload data. (see note 9)

                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    // 12. Part 13
                    new Claim(ClaimTypes.Role, user.Role) // when user is authenticated, we want to assign it's role inside it's token, so that we add that as a Claim here. We do that so we can use [Authorize(Roles = "")]

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature) // used to create a security token
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        // ---------------------


        // 12. Part 7
        // ---------------------

        public bool IsUserUnique(string username)
        {
            var userExists = _appDbContext.Users.Any(user => user.UserName == username);
            if(userExists)
            {
                return false;
            }
            return true;
        }

        public UserDto Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        // ---------------------
    }
    // ------------------------------
}
