using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParksModels.Dtos
{
    // My Modification

    public class NationalParkUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Established { get; set; }
    }
}
