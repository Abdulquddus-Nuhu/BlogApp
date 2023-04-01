using System;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbConnection = Environment.GetEnvironmentVariable("STERLOAN_DB_CONNECTION");
//Add Application Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(dbConnection);
});

builder.Services.AddIdentity<Persona, Role>(
         options =>
         {
             options.Password.RequireDigit = true;
             options.Password.RequireLowercase = true;
             options.Password.RequireNonAlphanumeric = true;
             options.Password.RequireUppercase = true;
             options.Password.RequiredLength = 8;
             options.User.RequireUniqueEmail = true;
             options.SignIn.RequireConfirmedEmail = true;
         })
         .AddEntityFrameworkStores<AppDbContext>()
         .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

