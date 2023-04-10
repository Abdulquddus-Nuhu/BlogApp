using System;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class DbSeedWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbSeedWorker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeedWorker>>();
            try
            {
                logger.LogInformation("Applying Migration!");
                await context.Database.MigrateAsync(cancellationToken: cancellationToken);
                logger.LogInformation("Migration Successful!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to apply Migration!");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}

