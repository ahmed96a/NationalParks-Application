using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ParksModels.Dtos;
using ParksModels.Models;
using ParksWeb.Repository;
using ParksWeb.Repository.IRepository;

namespace ParksWeb.Controllers
{
    // 10. Part 2
    // ----------------------------------
    
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IConfiguration _confg;
        private readonly string _trailUrl;
        private readonly string _npUrl;

        public TrailsController(INationalParkRepository nationalParkRepository, ITrailRepository trailRepository, IConfiguration configuration)
        {
            _trailRepo = trailRepository;
            _npRepo = nationalParkRepository;
            _confg = configuration;
            _trailUrl = _confg.GetSection("APIConstants").GetSection("TrailAPIPath").Value;
            _npUrl = _confg.GetSection("APIConstants").GetSection("NationalParkAPIPath").Value;
        }



        /* Note About Index()
        // In the tutorial he make Index action method return empty object. And make other async method that return the Trails object.
        // And that new method will be called by jQuery ajax from the view when call dataTables plugin. so he make that thing because he want to call the data from the datatable plugin.
        // As there is no reason for me to this, i worked in the normal way.
        */

        public async Task<IActionResult> Index()
        {
            var trailDtos = await _trailRepo.GetAllSync(_trailUrl, HttpContext.Session.GetString("JWToken"));
            return View(trailDtos);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var nationalParkDtos = await _npRepo.GetAllSync(_npUrl, HttpContext.Session.GetString("JWToken"));
            ViewBag.NationalParksSelectList = new SelectList(nationalParkDtos, "Id", "Name");

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrailCreateDto model)
        {
            if(ModelState.IsValid)
            {
                var success = await _trailRepo.CreateAsync(_trailUrl, model, HttpContext.Session.GetString("JWToken"));
                
                if(success)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "SomeError happens.");
            }
            return View(model);
        }


        
        [HttpGet]
        [Authorize(Roles = "Admin")] // 13. Part 9
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            TrailDto trailDto = await _trailRepo.GetAsync(_trailUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if(trailDto == null)
            {
                return NotFound();
            }

            var nationalParkDtos = await _npRepo.GetAllSync(_npUrl, HttpContext.Session.GetString("JWToken"));
            ViewBag.NationalParksSelectList = new SelectList(nationalParkDtos, "Id", "Name", trailDto.NationalParkId);

            TrailUpdateDto trailUpdateDto = new TrailUpdateDto()
            {
                 Id = trailDto.Id,
                 Name = trailDto.Name,
                 Distance = trailDto.Distance,
                 DifficultyType = trailDto.DifficultyType,
                 NationalParkId = trailDto.NationalParkId
            };

            return View(trailUpdateDto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrailUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var success = await _trailRepo.UpdateAsync(_trailUrl, model.Id, model, HttpContext.Session.GetString("JWToken"));

                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "SomeError happens.");
            }
            return View(model);
        }

        

        // 10. Part 7
        // ----------------------

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var trailDto = await _trailRepo.GetAsync(_trailUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if(trailDto == null)
            {
                return NotFound();
            }

            return View(trailDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(_trailUrl, id, HttpContext.Session.GetString("JWToken"));
            if(status)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        // ----------------------

    }

    // ----------------------------------
}
