using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jwt_test.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace jwt_test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            //create scope to use service
            using(var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;//get the service instance
                var _db = services.GetRequiredService<jwt_testDbContext>();
                try{
                    _db.Database.Migrate();
                    var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var _roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                    await SeedUserAccount.go(_userManager,_roleManager);
                    Console.WriteLine("Succeed --+++");
                }//end of try
                catch(Exception ex){
                    Console.WriteLine("Seeding Account Error",ex.Message);
                }//end of catch
            }//end of using

            host.Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
