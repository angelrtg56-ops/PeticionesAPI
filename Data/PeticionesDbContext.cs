using Microsoft.EntityFrameworkCore;
using PeticionesAPI.Models;

namespace PeticionesAPI.Data
{
    public class PeticionesDbContext : DbContext
    {
        public PeticionesDbContext(DbContextOptions<PeticionesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Peticion> Peticiones { get; set; }
    }
}