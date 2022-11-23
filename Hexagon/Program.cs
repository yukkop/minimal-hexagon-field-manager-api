using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hexagon.Configurations;
using Hexagon.Configurations.Autofac;
using Hexagon.Database.HexagonDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<HexagonContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Hexagon")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterAuth(builder.Configuration);
builder.Services.RegisterAutoMapper();

builder.RegisterAutofac();

var app = builder.Build();

using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
{
    if (scope is not null) 
        scope.ServiceProvider.GetRequiredService<HexagonContext>().Database.Migrate();
    else
        throw new Exception("Что то с миграциями...");
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

app.RegisterAuth();

app.UseAuthorization();

app.MapControllers();

app.Run();