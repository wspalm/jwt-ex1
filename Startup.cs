using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jwt_test.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace jwt_test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser,AppRole>()
             .AddEntityFrameworkStores<jwt_testDbContext>()
             .AddDefaultTokenProviders()
             .AddDefaultUI();
             
            services.AddControllersWithViews();
            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true) //web authen here
                // Adding Jwt Bearer  here
                .AddJwtBearer(options =>  
                {  
                    options.SaveToken = true;  
                    options.RequireHttpsMetadata = false;  
                    options.TokenValidationParameters = new TokenValidationParameters()  
                    {  
                        ValidateIssuer = true,  
                        ValidateAudience = true,  
                        ValidAudience = Configuration["JWT:Audience"],  
                        ValidIssuer = Configuration["JWT:Issuer"],  
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))  
                    };  
                }); 
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                 
            });  
            
            //setup database
            services.AddDbContextPool<jwt_testDbContext>(
                dbContextOptions => dbContextOptions
                .UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8,0,21)),
                    mySqlOptions => mySqlOptions
                    .CharSetBehavior(CharSetBehavior.NeverAppend)
                )
            );

            //add swagger to let me test easily
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions (apiDescriptions => apiDescriptions.First ());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "project api version1", Version = "v1" });
            });
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,jwt_testDbContext _context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //migrate
            _context.Database.Migrate();
            
            //swagger setup
            app.UseCors("p1");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "project api version1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            
            app.UseRouting();
            

            //make sure to use authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
