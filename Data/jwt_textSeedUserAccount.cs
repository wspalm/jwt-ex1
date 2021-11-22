using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;

namespace jwt_test.Data{
    public class SeedUserAccount{
        public static async Task go(UserManager<AppUser> _userManager , 
        RoleManager<AppRole> _roleManager){
            //create user role
            if(!await _roleManager.RoleExistsAsync("admin")){
                await _roleManager.CreateAsync(new AppRole("admin"));
            }//end of if
            //create admin user
            var superuser = new AppUser{
                UserName = "root@localhost.com",
                Email = "root@localhost.com",
                first_name = "super",
                last_name = "iamadmin"
            };//end of superuser object
            //query existing user
            if(_userManager.Users.All(u => u.UserName != superuser.UserName)){
                await _userManager.CreateAsync(superuser,"1234");
                Console.WriteLine("Root Account has been created --");
            }//end of if
            superuser = await _userManager.FindByEmailAsync("root@localhost.com");
            //insert role for super user
            if(!await _userManager.IsInRoleAsync(superuser,"admin")){
                await _userManager.AddToRoleAsync(superuser,"admin");
                Console.WriteLine("apply admin role to root ---");
            }//end of if
            else{
                Console.WriteLine("Admin user exist ====");
            }
        }//end of go function
    }//end of class
}//end of namespace