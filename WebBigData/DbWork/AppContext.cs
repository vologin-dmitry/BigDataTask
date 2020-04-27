using Microsoft.EntityFrameworkCore;

namespace WebBigData
{
    public class AppContext : DbContext
    {

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Actors { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
            Database.SetCommandTimeout(2147483646);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=bigdatadb;Trusted_Connection=True;");
        }

        public void ReCreate()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
