using Microsoft.EntityFrameworkCore;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;
using PRN231_Project.Repositories;

namespace PRN231_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<Project_PRN231Context>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ILabelRepository, LabelRepository>();
            builder.Services.AddScoped<IContact_LabelRepository, Contact_LabelRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
