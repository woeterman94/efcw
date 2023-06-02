// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfCore.Techorama
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var db = new WouterDataBaseContext();
            db.Database.EnsureDeleted(); // In real word scenarios use async!
            db.Database.EnsureCreated();
            db.Owners.Add(new Owner
            {
                FirstName = "Wouter",
                LastName = "V",
                Vehicle = new Train() { TrainTicket = "20233392943", MaxSpeed = 300 },
                Cats = new List<Cat>
            {
                new Cat { Name = "Garfield", SleepDuration = new Duration(10), FurColor = "Orange", OnADiet = true },
                new Cat { Name = "Floki",  SleepDuration = new Duration(14), FurColor = "Cinamon", OnADiet = false },
                new Cat { Name = "Charlie",  SleepDuration = new Duration(11), FurColor = "Brownie", OnADiet = false },


            }
            });

            db.Owners.Add(new Owner
            {
                FirstName = "Marcel",
                LastName = "De Bel",
                Vehicle = new Car() { CarKey = "GFDH32", MaxSpeed = 234 }
            });

            db.SaveChanges();

        }
    }


    class WouterDataBaseContext : DbContext
    {

        // multi tenany configuration here?

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
            modelBuilder.ApplyConfiguration(new CatConfiguration());
            modelBuilder.Entity<Vehicle>()
               .HasDiscriminator<string>("vehicleType")
               .HasValue<Train>("choochoo")
               .HasValue<Car>("vroomvroom");
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
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.OwnsOne(x => x.HomeAddress); // owned type
            builder.OwnsOne(x => x.ShippingAddress);
            builder.HasQueryFilter(x => x.Active); // kan je overschrijven met ignore query filter



        }
    }

    class CatConfiguration : IEntityTypeConfiguration<Cat>
    {
        public void Configure(EntityTypeBuilder<Cat> builder)
        {
            //builder.Property(x => x.DateOfBirth)
            //    .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction)
            //    .HasField("dob");

            builder.Property<DateTimeOffset>("LastUpdated"); // shadow property

            builder.Property(x => x.SleepDuration)
                .HasConversion(new DurationConverter());
        }
    }



    class Owner
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Address ShippingAddress { get; set; }
        public Address HomeAddress { get; set; }

        public Vehicle Vehicle { get; set; }


        public ICollection<Cat> Cats { get; set; }
    }

    class Address
    {
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    class Duration // value object?
    {
        private readonly int _value;
        public Duration(int ms)
        {
            _value = ms;
        }

        // duration.fromSeconds(...)

        public int Value => _value;
    }

    class DurationConverter : ValueConverter<Duration, int>
    {
        public DurationConverter() : base(d => d.Value, x => new Duration(x), null)
        {
        }
    }



    class Cat
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Race { get; set; }
        public int? Weight { get; set; }
        public bool? OnADiet { get; set; }
        public Duration SleepDuration { get; set; }
        public string? FavoriteFood { get; set; }
        public string? FurType { get; set; }
        public string? FurPattern { get; set; }
        public string? FurColor { get; set; }
        public int? FurSoftness { get; set; }
        public Owner Owner { get; set; }
        public string? Name { get; set; }
    }


    // inheritance

    class Vehicle
    {
        public Guid Id { get; set; }
        public int MaxSpeed { get; set; }
    }

    class Car : Vehicle
    {
        public string CarKey { get; set; }
    }

    class Train : Vehicle
    {
        public string TrainTicket { get; set; }
    }


}