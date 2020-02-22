using System;
using Microsoft.EntityFrameworkCore;
using Moment32.Models;

namespace Moment32.Data
{
    public class CdContext : DbContext
    {
        public CdContext(DbContextOptions<CdContext> options) : base(options)
        {
        }
        public DbSet<Cd> Cd { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Borrow> Borrow { get; set; }
    }
}
