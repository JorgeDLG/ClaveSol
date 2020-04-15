using Microsoft.EntityFrameworkCore;
using ClaveSol.Models;

namespace ClaveSol.Data
{
    public class MvcUserContext : DbContext
    {
        public MvcUserContext (DbContextOptions<MvcUserContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } //conjunto entidades (Tabla) # Entidad (fila)
    }
}