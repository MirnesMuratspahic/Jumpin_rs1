using AlarmSystem.Models;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jumpin.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //User section
        public DbSet<User> Users { get; set; }
        public DbSet<UserCode> UserCodes { get; set; }

        //Routes section
        public DbSet<TheRoute> Routes { get; set; }
        public DbSet<UserRoute> UserRoutes { get; set; }
        public DbSet<RouteRequest> RouteRequests { get; set; }

        //Cars section
        public DbSet<Car> Cars { get; set; }
        public DbSet<UserCar> UserCars { get; set; }
        public DbSet<CarRequest> CarRequests { get; set; }

        //Flats section
        public DbSet<Flat> Flats { get; set; }
        public DbSet<UserFlat> UserFlats { get; set; }
        public DbSet<FlatRequest> FlatRequests { get; set; }

        //Errors section
        public DbSet<CodeStatus> Errors { get; set; }

        //Rating section
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<UserRating> UserRating {  get; set; }

        // Images section

        public DbSet<VipImage> VipImages { get; set; }


    }

}
