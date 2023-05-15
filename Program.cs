// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
        }

        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Cat> Cats => Set<Cat>();

    }

    class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("people");
            builder.Property(x => x.LastName)
                .IsRequired()
                .IsFixedLength(false)
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("surname");
            builder.HasMany(x => x.Cats)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
        }
    }

    class CatConfiguration : IEntityTypeConfiguration<Cat>
    {
        public void Configure(EntityTypeBuilder<Cat> builder)
        {
            // builder.Property()
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
        public int OwnerId { get; set; }
        public string Race { get; set; }
        public int Weight { get; set; }
        public bool OnADiet { get; set; }
        public string FavoriteFood { get; set; }
        public string FurType { get; set; }
        public string FurPattern { get; set; }
        public string FurColor { get; set; }
        public int FurSoftness { get; set; }
        public Owner Owner { get; set; }
        public string Name { get; set; }
    }
}