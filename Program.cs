// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;


namespace EfCore.Techorama
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var db = new WouterDataBaseContext();
            db.Database.EnsureDeleted(); // In real word scenarios use async!
            db.Database.EnsureCreated();

        }
    }


    class WouterDataBaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=.;Database=Wouter;User Id=sa;Password=W@ffles!123;ConnectRetryCount=0;TrustServerCertificate=True");
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Cat> Cats { get; set; }

    }


    class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Cat> Cats { get; set; }
    }

    class Cat
    {
        public int Id { get; set; }
        public Owner Owner { get; set; }
        public string Name { get; set; }
    }
}