using Microsoft.EntityFrameworkCore;
using TranslateAPIgoogle.entities;

namespace TranslateAPIgoogle.Context
{
    public class AppDbContext:DbContext
    {
        public DbSet<Manager> managers { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Order> orders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-5BTSQ2P\\SQLEXPRESS;database=TranslateAPIgooogle;integrated security=sspi;");
        }
    }
}
