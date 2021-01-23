using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace ParksModels.Dtos
{
    // My Modification

    public class NationalParkCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Established { get; set; }
    }
}
