using ParksModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParksModels.Dtos
{
    // 6. Part 1
    // ------------------------

    public class TrailDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public DifficultyType DifficultyType { get; set; }

        public int NationalParkId { get; set; }

        public NationalParkDto NationalPark { get; set; }
    }

    // ------------------------
}
