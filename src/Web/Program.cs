using System;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Web;

var builder = WebApplication.CreateBuilder(args);

var dbConnection = Environment.GetEnvironmentVariable("BLOG_APP_DB_CONNECTION");

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

builder.Services.AddRazorPages(options =>
{
    //options.Conventions.AuthorizePage("/Index");
    options.Conventions.AuthorizeFolder("/Private");
    options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
    options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
});

builder.Services.AddAuthentication().AddTwitter(options =>
{
    options.ConsumerKey = builder.Configuration["TwitterAPIKey"];
    options.ConsumerSecret = builder.Configuration["TwitterAPIKeySecret"];
});

//Register Email Service
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Identity/Account/Login";
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opts.LogoutPath = "/Identity/Account/Logout";
});

// Register the worker responsible of seeding the database.
builder.Services.AddHostedService<DbSeedWorker>();

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7288;
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

