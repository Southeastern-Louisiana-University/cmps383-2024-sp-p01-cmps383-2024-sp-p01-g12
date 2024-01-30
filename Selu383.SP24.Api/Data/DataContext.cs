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
                

            modelBuilder.Entity<Hotel>()
                .HasData(
                new Hotel  { Id = 1, Name = "Hilton", Address = "123 Main St"},
                new Hotel { Id = 2, Name = "Easy Sleep", Address = "2200 South Rd"},
                new Hotel { Id = 3, Name = "Comfort Inn", Address = "380 North Cove"}
                ); 
        } 
    }
}
