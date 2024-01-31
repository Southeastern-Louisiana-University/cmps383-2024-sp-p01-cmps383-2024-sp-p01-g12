using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Features.Hotels;

namespace Selu383.SP24.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) 
        {
        }
       public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>()
                .Property(x => x.Name)
                .HasMaxLength(120);
        } 
    }
}
