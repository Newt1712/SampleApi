using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Web.Domains.Entities;
using Web.Application.DI;
using Web.Infrastructure.DependencyInjection;
using Web.Infrastructure.DBContext;




//-------------------------------------------- Builder --------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// Init auto mapper
builder.Services.InitAutoMapper();

// Init mem cache
builder.Services.AddMemoryCache();

// Init controllers
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Init db Context
builder.Services.AddDbContext(builder.Configuration);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Init services
builder.Services.AddServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();

// Init JWT
builder.Services.AddAuthentication(builder.Configuration);




//-------------------------------------------- App Builder --------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await AutomatedMigration.MigrateAsync(app.Services.CreateScope().ServiceProvider);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
