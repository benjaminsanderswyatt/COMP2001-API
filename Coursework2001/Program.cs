using Coursework2001.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add mvc services
builder.Services.AddControllersWithViews();

//add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//add entity framework core
builder.Services.AddDbContext<COMP2001_BSanderswyattContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB_2001"))
);

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
