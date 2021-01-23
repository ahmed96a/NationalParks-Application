using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using ParksModels.Dtos;
using ParksAPI.Models.Repository;
using ParksAPI.Models.Repository.IRepository;
using ParksModels.Models;
using Microsoft.AspNetCore.Authorization;

namespace ParksAPI.Controllers
{
    // 6. Part 3
    // -----------------------------------

    //[Route("api/[controller]")] // 7. Part 4

    // This will dynamically load the version that this controller belongs to.
    // We have the 'v' and then the version will get it from the ApiVersion attribute. if version isn't defined using [ApiVersion], then it will be v1.0 by default (we set in the configuration of AddApiVersioning).
    
    [Route("api/v{version:apiVersion}/[controller]")] // 7. Part 4
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParksOpenAPISpecTrails")] // 7. Part 1, specify that this Api Controller belongs to that OAS (OenAPI Specification)
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[Authorize]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository trailRepository;
        private readonly IMapper mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            this.trailRepository = trailRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get list of Trails.
        /// </summary>
        /// <returns>It Returns List of TrailDto</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var trails = trailRepository.GetTrails();
            var trailDtos = new List<TrailDto>();

            // Convert Trail object (Domain Model) to TrailDto (Dto)
            foreach (var trail in trails)
            {
                trailDtos.Add(mapper.Map<TrailDto>(trail));
            }

            return Ok(trailDtos);
        }



        [HttpGet("trailsInPark/{npId:int}")] // 7. Part 6
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult GetTrailsInNationalPark(int npId)
        {
            // 7. Part 6
            // ---------------------------

            var trails = trailRepository.GetTrailsInNationalPark(npId);

            if(trails == null)
            {
                return NotFound($"National Park with id:{npId}  not exist.");
            }

            var trailsDtos = new List<TrailDto>();

            foreach (var trail in trails)
            {
                var trailDto = mapper.Map<TrailDto>(trail);
                trailsDtos.Add(trailDto);
            }

            return Ok(trailsDtos);

            // ---------------------------
        }



        /// <summary>
        /// Get Individual Trail.
        /// </summary>
        /// <param name="trailId">The Id of the Trail.</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType()]
        [Authorize(Roles = "Admin")] // 12. Part 13
        public IActionResult GetTrail(int trailId)
        {
            var trail = trailRepository.GetTrail(trailId);
            
            if(trail == null)
            {
                return NotFound();
            }

            // Convert Trail object (Domain Model) to TrailDto (Dto)
            var trailDto = mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Trail))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailCreateDto)
        {
            // if trailDto == null
            if (trailCreateDto == null) // 6. Part 4
            {
                return BadRequest();
            }

            // if Trail already exists, add error to the modelstate
            if (trailRepository.TrailExists(trailCreateDto.Name)) // 6. Part 4
            {
                ModelState.AddModelError("", "Trail already exists!");
                return BadRequest(ModelState); // In the tutorial, we returned 404 status code, which isn't convinced to me "return StatusCode(404, ModelState);".
            }

            #region ModelState
            // We don't need that piece of code, because if the model state is not valid it will automatically return badrequest with the model validations messages.
            /*
            // We comment that block of code in "4. Part 6"

            // if ModelState not valid
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            */
            #endregion

            var trail = mapper.Map<Trail>(trailCreateDto); // 6. Part 4

            if (!trailRepository.CreateTrail(trail)) // trail variable after Creation, will be populated with the Id
            {
                ModelState.AddModelError("", $"Something went wrong, when saving the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            var trailDto = mapper.Map<TrailDto>(trail); // My Modification, we return trailDto instead of trail, because we shouldn't reveal our Domain Models outside.

            return CreatedAtRoute("GetTrail", new { version = HttpContext.GetRequestedApiVersion().ToString(), trailId = trail.Id }, trailDto);
        }

        
        [HttpPut("{trailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailUpdateDto)
        {
            if(trailUpdateDto == null || trailId != trailUpdateDto.Id) // 6. Part 5
            {
                return BadRequest();
            }

            if(!trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var trail = mapper.Map<Trail>(trailUpdateDto); // 6. Part 5

            if (!trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong, when updating the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        
        [HttpDelete("{trailId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var trail = trailRepository.GetTrail(trailId);

            if (!trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong, when deleting the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }

    // -----------------------------------
}
