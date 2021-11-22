using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using jwt_test.Models;
using jwt_test.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace jwt_test.Controllers{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller{
        //define necessary object to be able to use
        //dependencies injection
        public IConfiguration _configuration;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private jwt_testDbContext _db;

        //take 4 inputs
        public AccountController(
            IConfiguration configuration,
            jwt_testDbContext db,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager
        ){
            _db = db;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;

        }//end of contructor function

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserData model){
            //validate nulltiy
            if(model != null  && model.username !=null && model.password !=null ){
                return await authenticate(model);
                 
            }//end if
            else{
                //400 code
                //return BadRequest();
                return Json(new {
                    status_code = 400,
                    message = "bad request"
                });
            }
        }//ef
     

        private async Task<IActionResult> authenticate(UserData model){
              var user = await _userManager.FindByNameAsync(model.username);  
              
            if (user != null && await _userManager.CheckPasswordAsync(user, model.password))  
            {  
                var userRoles = await _userManager.GetRolesAsync(user);  
   
                var authClaims = new List<Claim>  
                {  
                    new Claim(ClaimTypes.Name, user.UserName),  
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
                };  
  
                foreach (var userRole in userRoles)  
                {  
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
                }  
  
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));  
  
                var token = new JwtSecurityToken(  
                    issuer: _configuration["JWT:Issuer"],  
                    audience: _configuration["JWT:Audience"],  
                    expires: DateTime.Now.AddHours(1),  
                    claims: authClaims,  
                    signingCredentials: 
                    new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
                    );  
  
                return Ok(new  
                {  
                    roles = userRoles,
                    token = new JwtSecurityTokenHandler().WriteToken(token),  
                    expiration = token.ValidTo  
                });  
            }
            
             return Unauthorized(); 
             //return Json(new {
             //       status_code = 401,
             //       message = "Unauthorized"
             //});
        }//ef

        [HttpPost]  
        public async Task<IActionResult> Register([FromBody] RegisterData model)  
        {  
            var userExists = await _userManager.FindByNameAsync(model.username);  
            if (userExists != null)  
                return StatusCode(StatusCodes.Status500InternalServerError, 
                new  { Status = "Error", Message = "User already exists!" });  
  
            AppUser user = new AppUser()  
            {  
                
                Email = model.email,  
                SecurityStamp = Guid.NewGuid().ToString(),  
                UserName = model.username  
            };  
            
            var result = await _userManager.CreateAsync(user, model.password);  
            if (!result.Succeeded)  
                return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Status = "Error", Message = "User creation failed! Please check user details and try again." });  
  
            return Ok(new  { Status = "Success", Message = "User created successfully!" });  
        }//ef
    }//end of class
}//end of controllers