using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using jwt_test.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using jwt_test.Models;
using System;

namespace jwt_test.Controllers{
    [Authorize(AuthenticationSchemes = 
    JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[action]")]
    public class ApiController : Controller{
        private jwt_testDbContext _db;
        public ApiController(jwt_testDbContext db){
            _db = db;
        }//end of contructor function
        [HttpGet]
        public IActionResult helloworld(){
            return Json(new {
                status_code = 200,
                message = "hello world"
            });
        }//end of helloworld function
    }//end of class
}//end of namespace

