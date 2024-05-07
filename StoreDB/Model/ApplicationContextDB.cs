using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace OnlineStoreDB.Model
{
    public class ApplicationContextDB : DbContext
    {
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Offices> Offices { get; set; }
        public DbSet<Order_Items> OrderItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Sellers> Sellers { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"Server=.;Database=OnlineStoreDB12;Trusted_Connection=True;TrustServerCertificate=True");
        }

        private static ApplicationContextDB instance;
        public static ApplicationContextDB getInstance()
        {
            if (instance == null)
            {
                instance = new ApplicationContextDB();
                instance.Database.EnsureCreated();

                instance.Customers.Load();
                instance.Employees.Load();
                instance.Offices.Load();
                instance.OrderItems.Load();
                instance.Orders.Load();
                instance.Positions.Load();
                instance.Products.Load();
                instance.Sellers.Load();
                instance.Users.Load();
               
                instance.SaveChanges();
            }
            return instance;
        }

       
    }
}
