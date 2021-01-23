using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ParksModels.Dtos;
using ParksWeb.Repository.IRepository;

namespace ParksWeb.Controllers
{
    // 13. Part 3
    // -------------------

    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IConfiguration _confg;
        private readonly string _accountUrl;

        public AccountController(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepo = accountRepository;
            _confg = configuration;
            _accountUrl = _confg.GetSection("APIConstants").GetSection("AccountAPIPath").Value;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                // HttpContext property that we use to access Session is available only in the Controller, so that we can't use it in the repository class.
                var userDto = await _accountRepo.LoginAsync(_accountUrl + "authenticate", model, HttpContext.Session.GetString("JWToken"));
                
                if (userDto == null)
                {
                    ModelState.AddModelError("", "Error occured");
                    return View(model);
                }

                // 13. Part 8
                // -----------------------

                // To be able to use the cookie authentication we have to add a PrincipleClaim and a claim.

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                // Here we add Claims to that ClaimsIdentity variable, so we can use it throughout our application. we will add Username and role to our ClaimIdentity
                identity.AddClaim(new Claim(ClaimTypes.Name, userDto.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, userDto.Role));
                // Claims are very important, to be able to use role based authorization, we should Add Claim Role, (That is the same with JWT token authentication, to be able to use role based authorization, we had to add role claim in the subject property of token descriptor, see 12. Part 13)

                // create ClaimsPrincipal
                var principal = new ClaimsPrincipal(identity); // User property, represents that ClaimsPrincipal variable.
                
                // the last step is to sign the user in
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                

                // -----------------------

                HttpContext.Session.SetString("JWToken", userDto.Token); // That is needed for API Calls

                // temp data is used to store the data in something like a session but only for one request. So if a page goes from one page to one other it will have those information. But if you refresh the page that will go away
                TempData["alert"] = "Login Successfully."; // 13. Part 10

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // 13. Part 4
        // -------------------

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepo.RegisterAsync(_accountUrl + "register", model, HttpContext.Session.GetString("JWToken"));

                if (!result)
                {
                    ModelState.AddModelError("", "Error occured");
                    return View(model);
                }

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            // 13. Part 8
            // -----------------------

            await HttpContext.SignOutAsync();

            // -----------------------

            HttpContext.Session.SetString("JWToken", ""); // can i use "HttpContext.Session.Remove("JWToken");"

            // temp data is used to store the data in something like a session but only for one request. So if a page goes from one page to one other it will have those information. But if you refresh the page that will go away
            TempData["alert"] = "Registeration Successfully."; // 13. Part 10

            return RedirectToAction("Index", "Home");
        }

        // -------------------
    }

    // -------------------
}
