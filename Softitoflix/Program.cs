using Microsoft.EntityFrameworkCore;
using Softitoflix.Data;
using Microsoft.AspNetCore.Identity;
using Softitoflix.Models;


namespace Softitoflix;

public class Program
{
    public static void Main(string[] args)
    {
        SoftitoflixRole softitoflixRole;
        SoftitoflixUser softitoflixUser;

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<SoftitoflixContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SoftitoflixContext") ?? throw new InvalidOperationException("Connection string 'SoftitoflixContext' not found.")));

        builder.Services.AddIdentity<SoftitoflixUser, SoftitoflixRole>()
            .AddEntityFrameworkStores<SoftitoflixContext>().AddDefaultTokenProviders();


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication(); 
        app.UseAuthorization();


        app.MapControllers();

        SoftitoflixContext? context = app.Services.CreateScope().ServiceProvider.GetService<SoftitoflixContext>();
        if (context != null)
        {
            context.Database.Migrate();
            context.SaveChanges();
            RoleManager<SoftitoflixRole>? roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<SoftitoflixRole>>();
            if (roleManager != null)
            {
                if(roleManager.Roles.Count() == 0)
                {
                    softitoflixRole = new SoftitoflixRole("Admin");
                    roleManager.CreateAsync(softitoflixRole).Wait();
                    softitoflixRole = new SoftitoflixRole("ContentAdmin");
                    roleManager.CreateAsync(softitoflixRole).Wait();
                }
            }

            UserManager<SoftitoflixUser>? userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<SoftitoflixUser>>();
            if (userManager != null) 
            {
                if(userManager.Users.Count() == 0)
                {
                    softitoflixUser = new SoftitoflixUser();
                    softitoflixUser.UserName = "Admin";
                    softitoflixUser.Name = "Admin";
                    softitoflixUser.Email = "admin@softitoflix.com";
                    softitoflixUser.PhoneNumber = "1234567890";
                    softitoflixUser.BirthDate = DateTime.Today;
                    softitoflixUser.isPassive = false;
                    userManager.CreateAsync(softitoflixUser, "Admin123!").Wait();
                    userManager.AddToRoleAsync(softitoflixUser, "Admin").Wait();
                }
            }
        }

        app.Run();
    }
}

