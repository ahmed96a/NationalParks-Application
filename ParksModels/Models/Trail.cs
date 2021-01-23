using ParksModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParksModels.Models
{
    // 6. Part 1
    // -------------------

    public class Trail
    {
        [Key] // we can neglect that attribute, since by default, a property with name "Id" will be considered as a primary key with Identity
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public DifficultyType DifficultyType { get; set; }

        // [Required] // There is no need to that attribute, because see note 1
        public int NationalParkId { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [ForeignKey("NationalParkId")]
        public NationalPark NationalPark { get; set; }
    }

    // -------------------
}
