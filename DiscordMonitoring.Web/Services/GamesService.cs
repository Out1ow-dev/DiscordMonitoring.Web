using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;
using Microsoft.EntityFrameworkCore;
using SteamQuery;

namespace DiscordMonitoring.Web.Services
{
    public class GamesService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public GamesService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(PerformMonitoring, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void PerformMonitoring(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DiscordMonitoring.Web.Data.ApplicationDbContext>();
                var servers = await context.Servers.Include(s => s.Game).ToListAsync();
                var gameGroups = servers
                    .GroupBy(s => s.GameId)
                    .Select(g => new
                    {
                        GameId = g.Key,
                        ServerCount = g.Count(),
                        TotalOnline = g.Sum(s => int.TryParse(s.ServerOnline.Split("/").First(), out var online) ? online : 0)
                    });

                foreach (var gameGroup in gameGroups)
                {
                    var game = await context.Games.FindAsync(gameGroup.GameId);
                    if (game != null)
                    {
                        game.ServerCount = gameGroup.ServerCount;
                        game.GeneralOnline = gameGroup.TotalOnline;
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
