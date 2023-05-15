// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;


namespace EfCore.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
        }
    }


    class WouterDataBaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
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