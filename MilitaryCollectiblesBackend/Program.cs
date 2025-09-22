
using Microsoft.EntityFrameworkCore;
using MilitaryCollectiblesBackend.Data;
using MilitaryCollectiblesBackend.DataAccessLayer;

namespace MilitaryCollectiblesBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddDbContext<MilitaryCollectiblesDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.
                GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddControllers();
        builder.Services.AddDataAccessLayerServices();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.MapDefaultEndpoints();

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
