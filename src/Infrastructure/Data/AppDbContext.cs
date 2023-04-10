using System;
using Core.Entities;
using Infrastructure.EntityConfigurations;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<Persona, Role, Guid>
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _options = options;
        }

        public DbSet<Blog> Blogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(BlogEfConfig).Assembly);
        }

        public async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}

