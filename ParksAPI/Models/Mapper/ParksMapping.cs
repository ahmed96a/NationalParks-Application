using AutoMapper;
using ParksModels.Dtos;
using ParksModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.Mapper
{
    // 3. Part 5
    // -----------------------------

    // Here we will define the mappings that we want to create in our project.
    public class ParksMapping : Profile
    {
        public ParksMapping()
        {
            // "ReverseMap()" => This means it will map in the both ways, it will map NationalPark to NationalParkDto and it would also map NationalParkDto object to NationalPark.
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();

            CreateMap<Trail, TrailDto>().ReverseMap(); // 6.Part 3
            CreateMap<Trail, TrailCreateDto>().ReverseMap(); // 6. Part 4
            CreateMap<Trail, TrailUpdateDto>().ReverseMap(); // 6. Part 5


            // My Modification
            CreateMap<NationalPark, NationalParkCreateDto>().ReverseMap();
            CreateMap<NationalPark, NationalParkUpdateDto>().ReverseMap();

            // 12. Part 9, My Modification
            // -------------------------------

            CreateMap<User, UserDto>().ReverseMap();

            // -------------------------------

            CreateMap<User, RegisterDto>().ReverseMap(); // 12. Part 12
        }
    }

    // -----------------------------
}
