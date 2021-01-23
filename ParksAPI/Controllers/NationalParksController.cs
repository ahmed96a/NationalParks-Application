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
    // 4. Part 1
    // -----------------------------------

    //[Route("api/[controller]")] // 7. Part 4

    // This will dynamically load the version that this controller belongs to.
    // We have the 'v' and then the version will get it from the ApiVersion attribute. if version isn't defined using [ApiVersion], then it will be v1.0 by default (we set in the configuration of AddApiVersioning).
    // We have to replace [controller] with the name of the controller (NationalParks) because with [controller] we will have two individual routes, one for NationalParks and the other for NationalParksV2.
    // When we use NationalParks instead of [controller], then we will have the same route for the two controllers, if we select V 1.0, that NationalParksController, if we selected V 2.0, NationalParksControllerV2 will be selected.

    [Route("api/v{version:apiVersion}/NationalParks")] // 7. Part 4

    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParksOpenAPISpecNP")] // 7. Part 1, specify that this Api Controller belongs to that OAS (OenAPI Specification)
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // 5. Part 8 // That response type will be applied on all the action methods.
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository npRepository;
        private readonly IMapper mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            npRepository = nationalParkRepository;
            this.mapper = mapper;
        }

        // 4. Part 2
        // -----------------------

        // 5. Part 5
        /// <summary>
        /// Get list of National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))] // 5. Part 8
        //[ProducesResponseType(400)] // 5. Part 8 // That response type applied on the controller level, so we didn't need to use it again here.
        public IActionResult GetNationalParks()
        {
            var nationalParks = npRepository.GetNationalParks();

            // return Ok(nationalParks);

            /*
             But there is one big mistake that we have made here, and that is we are returning a list of NationalPark (Domain Model) object.
             We should using DTOs. We should never expose our domain model to the outside world.
             We should only expose the DTOs, so that means we have to convert this NationalParks into a list of NationalParksDto.
            */

            var npDtos = new List<NationalParkDto>();

            // Convert NationalPark object (Domain Model) to NationalParkDto (Dto)
            foreach (var nationalPark in nationalParks)
            {
                npDtos.Add(mapper.Map<NationalParkDto>(nationalPark));
            }

            return Ok(npDtos);
        }

        // -----------------------

        // 4. Part 3
        // -----------------------

        // We can specifie the route template in the HttpGet attribute, or we can specify it in Route attribute. we can specify the route value type or neglect it.
        //[HttpGet("{nationalParkId}", Name = "GetNationalPark")]

        // 5. Part 5
        /// <summary>
        /// Get Individual National Park.
        /// </summary>
        /// <param name="nationalParkId">The Id of the National Park.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))] // 5. Part 8
        [ProducesResponseType(404)] // 5. Part 8
        [ProducesDefaultResponseType()] // 5. Part 8, see note  for explanation
        [Authorize] // 12. Part 2
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = npRepository.GetNationalPark(nationalParkId);
            
            if(nationalPark == null)
            {
                return NotFound();
            }

            // Convert NationalPark object (Domain Model) to NationalParkDto (Dto)
            var npDto = mapper.Map<NationalParkDto>(nationalPark);
            return Ok(npDto);
        }

        // -----------------------

        // 4. Part 4
        // -----------------------

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalPark))] // 5. Part 8
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // 5. Part 8
        public IActionResult CreateNationalPark([FromBody] NationalParkCreateDto nationalParkCreateDto)
        {
            // if nationalParkDto == null
            if (nationalParkCreateDto == null)
            {
                return BadRequest();
            }

            // if NationalPark already exists, add error to the modelstate
            if (npRepository.NationalParkExists(nationalParkCreateDto.Name))
            {
                ModelState.AddModelError("", "National Park already exists!");
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

            var nationalPark = mapper.Map<NationalPark>(nationalParkCreateDto);

            if (!npRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong, when saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            //return Ok();

            return CreatedAtRoute("GetNationalPark", new { version = HttpContext.GetRequestedApiVersion().ToString(), // 7. Part 5
                                                           nationalParkId = nationalPark.Id }, nationalPark); // 4. Part 6
        }

        // -----------------------

        // I used Put, instead of Patch, and i changed the logic of the action method than in the tutorial.
        // 4. Part 7
        // -----------------------

        [HttpPut("{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // 5. Part 8
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 5. Part 8
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // 5. Part 8
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody]NationalParkUpdateDto nationalParkUpdateDto)
        {
            if(nationalParkUpdateDto == null || nationalParkId != nationalParkUpdateDto.Id)
            {
                return BadRequest();
            }

            if(!npRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalPark = mapper.Map<NationalPark>(nationalParkUpdateDto);

            if (!npRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong, when updating the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // -----------------------

        // 4. Part 8
        // -----------------------

        [HttpDelete("{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // 5. Part 8
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 5. Part 8
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // 5. Part 8
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!npRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalPark = npRepository.GetNationalPark(nationalParkId);

            if (!npRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong, when deleting the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // -----------------------

    }

    // -----------------------------------
}
