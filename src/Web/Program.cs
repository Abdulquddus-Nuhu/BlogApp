using System;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Web;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

//Add Application Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("BLOG_APP_DB_CONNECTION"));
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
        options.SignIn.RequireConfirmedEmail = false;
    })
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Index");
});

builder.Services.AddAuthentication().AddTwitter(options =>
{
    options.ConsumerKey = builder.Configuration["TwitterAPIKey"];
    options.ConsumerSecret = builder.Configuration["TwitterAPIKeySecret"];
});

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Identity/Account/Login";
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opts.LogoutPath = "/Identity/Account/Logout";
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7288;
});

//Register Email Service
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// Register the worker responsible of seeding the database.
builder.Services.AddHostedService<DbSeedWorker>();


//Logging
builder.Services.AddLogging(x => {
    x.ClearProviders();
    x.AddConfiguration(builder.Configuration.GetSection("Logging"));
    x.AddConsole();
    x.AddJsonConsole(jsonLog =>
    {
        jsonLog.JsonWriterOptions = new System.Text.Json.JsonWriterOptions()
        {
            Indented = true,
        };
    });
    
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

