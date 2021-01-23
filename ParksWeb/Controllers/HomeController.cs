using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParksWeb.Models;
using ParksWeb.Repository.IRepository;
using ParksWeb.ViewModels;

namespace ParksWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // 11. Part 2
        // ------------------------

        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IConfiguration _confg;
        private readonly string _trailUrl;
        private readonly string _npUrl;

        // ------------------------

        public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepository, ITrailRepository trailRepository, IConfiguration configuration)
        {
            _logger = logger;

            // 11. Part 2
            // ------------------------

            _trailRepo = trailRepository;
            _npRepo = nationalParkRepository;
            _confg = configuration;
            _trailUrl = _confg.GetSection("APIConstants").GetSection("TrailAPIPath").Value;
            _npUrl = _confg.GetSection("APIConstants").GetSection("NationalParkAPIPath").Value;

            // ------------------------
        }

        /*
        public IActionResult Index()
        {
            return View();
        }
        */

        // 11. Part 2
        // ----------------------------

        public async Task<IActionResult> Index()
        {
            var asd = HttpContext.Session.GetString("JWToken");

            IndexVM indexVM = new IndexVM()
            {
                NationalParkList = await _npRepo.GetAllSync(_npUrl, HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepo.GetAllSync(_trailUrl, HttpContext.Session.GetString("JWToken"))
            };

            return View(indexVM);
        }

        // ----------------------------

        // 13. Part 8
        // ----------------------------

        
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ----------------------------

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
