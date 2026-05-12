using Login_Encriptacion.Models;
using Microsoft.EntityFrameworkCore;

namespace Login_Encriptacion.Data
{
    public class EncriptacionDbContext : DbContext
    {
        public EncriptacionDbContext(DbContextOptions<EncriptacionDbContext> opc) : base(opc) {}
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
