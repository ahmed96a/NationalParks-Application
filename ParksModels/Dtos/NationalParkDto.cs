using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParksModels.Dtos
{
    // 3. Part 4
    // -----------------------------

    public class NationalParkDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Established { get; set; }

        // We Comment that property, to avoid Reference Loop Cycle, see note in Notes folder of udemy course
        //public ICollection<TrailDto> Trails { get; set; }
    }

    // -----------------------------
}
