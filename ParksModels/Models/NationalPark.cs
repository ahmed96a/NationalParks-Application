using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParksModels.Models
{
    // 2. Part 1
    // ---------------------------------------

    public class NationalPark // Not forget to append public to the class, so we can use it in other namespaces. (See Note 2)
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Established { get; set; }

        public ICollection<Trail> Trails { get; set; }
    }

    // ---------------------------------------
}
