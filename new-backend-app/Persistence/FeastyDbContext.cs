using System;
using Microsoft.EntityFrameworkCore;
using new_backend.Models;

namespace new_backend.Data
{
    public class FeastyDbContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }

        public FeastyDbContext(DbContextOptions<FeastyDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}

