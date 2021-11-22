using jwt_test.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace jwt_test.Data{
    public class jwt_testDbContext : IdentityDbContext<AppUser,AppRole,int>{
        public jwt_testDbContext(DbContextOptions<jwt_testDbContext> options):base(options){

        }//end of contructor function
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<AppUser> AppUsers {get;set;}
        public DbSet<Car> Cars {get;set;}
        public DbSet<Driver> Drivers {get;set;}
    }//end of class
}//end of namespace