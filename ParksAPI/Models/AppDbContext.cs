using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParksModels.Models;


namespace ParksAPI.Models
{
    // 2. Part 2
    // -------------------------------------

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        // Add NationalParks table in our database.
        public DbSet<NationalPark> NationalParks { get; set; }

        public DbSet<Trail> Trails { get; set; } // 6. Part 2

        public DbSet<User> Users { get; set; } // 12. Part 3
    }

    // -------------------------------------
}
