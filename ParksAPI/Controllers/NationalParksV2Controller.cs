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

namespace ParksAPI.Controllers
{
    // 7. Part 2
    // -----------------------------------

    // [Route("api/[controller]")] // 7. Part 4

    // This will dynamically load the version that this controller belongs to.
    // We have the 'v' and then the version will get it from the ApiVersion attribute (which in our case will be 2.0). if version isn't defined using [ApiVersion], then it will be v1.0 by default (we set in the configuration of AddApiVersioning).
    // We have to replace [controller] with the name of the controller (NationalParks) because with [controller] we will have two individual routes, one for NationalParks and the other for NationalParksV2.
    // When we use NationalParks instead of [controller], then we will have the same route for the two controllers, if we select V 1.0, that NationalParksController, if we selected V 2.0, NationalParksControllerV2 will be selected.

    [Route("api/v{version:apiVersion}/NationalParks")] // 7. Part 4
    //[Route("api/NationalParks")]
    [ApiVersion("2.0")] // 7. Part 4

    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository npRepository;
        private readonly IMapper mapper;

        public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            npRepository = nationalParkRepository;
            this.mapper = mapper;
        }


        /// <summary>
        /// Get list of National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationalPark = npRepository.GetNationalParks().FirstOrDefault();

            return Ok(mapper.Map<NationalParkDto>(nationalPark));
        }

    }

    // -----------------------------------
}
