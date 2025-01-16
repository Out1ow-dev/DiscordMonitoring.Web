using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SteamQuery;
using static System.Formats.Asn1.AsnWriter;

namespace DiscordMonitoring.Web.Services
{
    public class MonitoringService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public MonitoringService(IServiceScopeFactory scopeFactory)
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
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var servers = await context.Servers.ToListAsync();

                var tasks = servers.Select(server => GetPlayersOnline(server, context));
                await Task.WhenAll(tasks);
            }
        }

        private async Task GetPlayersOnline(ServerEntity server, ApplicationDbContext context)
        {
            if (server.QueryPort == null || server.ServerIP == null)
            {
                return;
            }

            var queryServer = new GameServer($"{server.ServerIP}:{server.QueryPort}")
            {
                SendTimeout = 5000,
                ReceiveTimeout = 5000
            };

            bool isAvailable;
            try
            {
                var inf = await queryServer.GetInformationAsync();
                isAvailable = true;
            }
            catch
            {
                isAvailable = false;
            }

            try
            {
                if (isAvailable)
                {
                    await queryServer.PerformQueryAsync();
                    var information = await queryServer.GetInformationAsync();

                    if (information.OnlinePlayers == null || information.MaxPlayers == null)
                    {
                        return;
                    }

                    var game = await context.Games.FirstOrDefaultAsync(x => x.GameSteamId == information.GameId);
                    server.ServerMap = information.Map;
                    server.ServerVersion = information.Version;
                    server.ServerName = information.ServerName;
                    server.ServerOnline = $"{information.OnlinePlayers} / {information.MaxPlayers}";
                    server.Location = await GetCountryByIp.GetCountryAsync(server.ServerIP);
                    server.GamePort = information.Port.ToString();
                    server.Game = game;
                    server.IsOnline = true;

                    var onlineLog = new OnlineLog
                    {
                        OnlinePlayers = $"{information.OnlinePlayers} / {information.MaxPlayers}",
                        Date = DateTime.Now,
                    };
                    server.OnlineLog = new List<OnlineLog> { onlineLog };

                }
                else
                {
                    var onlineLog = new OnlineLog
                    {
                        OnlinePlayers = $"0 / 0",
                        Date = DateTime.Now,
                    };
                    server.OnlineLog = new List<OnlineLog> { onlineLog };
                    server.IsOnline = false;
                }
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                queryServer.Close();
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
