using Microsoft.EntityFrameworkCore;


namespace DBAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Meta> Metas { get; set; }

        public DbSet<Tarea> Tareas { get; set; }
    }


}
