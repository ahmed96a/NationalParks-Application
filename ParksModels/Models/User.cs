using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParksModels.Models
{
    // 12. Part 3
    // --------------------

    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }

    // --------------------
}
