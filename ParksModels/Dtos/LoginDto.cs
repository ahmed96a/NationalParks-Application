using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParksModels.Dtos
{
    // 12. Part 8
    // -------------------

    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    // -------------------
}
