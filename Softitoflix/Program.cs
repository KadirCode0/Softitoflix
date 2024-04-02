using Microsoft.EntityFrameworkCore;
using Softitoflix.Data;
using Microsoft.AspNetCore.Identity;
using Softitoflix.Models;


namespace Softitoflix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<SoftitoflixContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SoftitoflixContext") ?? throw new InvalidOperationException("Connection string 'SoftitoflixContext' not found.")));


        builder.Services.AddDefaultIdentity<SoftitoflixUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<SoftitoflixContext>();


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

        app.Run();
    }
}

