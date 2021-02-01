using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ParksModels.Dtos;
using ParksModels.Models;
using ParksWeb.Repository;
using ParksWeb.Repository.IRepository;

namespace ParksWeb.Controllers
{
    // 9. Part 1
    // ----------------------------------

    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IConfiguration _confg;
        private readonly string _npUrl;

        public NationalParksController(INationalParkRepository nationalParkRepository, IConfiguration configuration)
        {
            _npRepo = nationalParkRepository;
            _confg = configuration;
            _npUrl = _confg.GetSection("APIConstants").GetSection("NationalParkAPIPath").Value;
        }



        /* Note About Index()
        // In the tutorial he make Index action method return empty object. And make other async method that return the nationalParks object.
        // And that new method will be called by jQuery from the view when configure dataTables plugin. so he make that thing because he want to call the data from the datatable plugin.
        // As there is no reason for me to this, i worked in the normal way.
        */

        // 9. Part 1
        // ----------------
        public async Task<IActionResult> Index()
        {
            var nationalParkDtos = await _npRepo.GetAllSync(_npUrl, HttpContext.Session.GetString("JWToken"));
            return View(nationalParkDtos);
        }
        // ----------------



        // 9. Part 4
        // ----------------
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // ----------------


        // 9. Part 8
        // ----------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NationalParkCreateDto model, IFormFile Picture)
        {
            if(ModelState.IsValid)
            {
                //var files = HttpContext.Request.Form.Files; // to get the uploaded files without specify parameter to recieve them
                if(Picture != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Picture.CopyTo(ms);
                        model.Picture = ms.ToArray();
                    }
                }

                var success = await _npRepo.CreateAsync(_npUrl, model, HttpContext.Session.GetString("JWToken"));
                
                if(success)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "SomeError happens.");
            }
            return View(model);
        }

        // ----------------


        // 9. Part 4
        // ----------------
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            NationalParkDto nationalParkDto = await _npRepo.GetAsync(_npUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if(nationalParkDto == null)
            {
                return NotFound();
            }

            // NationalParkDto and NationalParkUpdateDto have the same properties, but we create both of them for the seprate of concerns.
            NationalParkUpdateDto nationalParkUpdateDto = new NationalParkUpdateDto()
            {
                Id = nationalParkDto.Id,
                Name = nationalParkDto.Name,
                State = nationalParkDto.State,
                Picture = nationalParkDto.Picture,
                Established = nationalParkDto.Established
            };

            return View(nationalParkUpdateDto);
        }
        // ----------------


        // 9. Part 8
        // ----------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NationalParkUpdateDto model, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        PictureFile.CopyTo(ms);
                        model.Picture = ms.ToArray();
                    }
                }

                var success = await _npRepo.UpdateAsync(_npUrl, model.Id, model, HttpContext.Session.GetString("JWToken"));

                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "SomeError happens.");
            }
            return View(model);
        }

        // ----------------

        // 9. Part 9
        // ----------------

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var status = await _npRepo.DeleteAsync(_npUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful"  });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        // ----------------
    }

    // ----------------------------------
}
