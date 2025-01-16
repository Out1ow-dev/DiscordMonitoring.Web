using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;
using SteamQuery;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using DiscordMonitoring.Web.Services;

namespace DiscordMonitoring.Web.Pages.ServerControl
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "GameName");
            return Page();
        }

        [BindProperty]
        public ServerEntity ServerEntity { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var server = new GameServer($"{ServerEntity.ServerIP}:{ServerEntity.QueryPort}")
            {
                SendTimeout = 10000,
                ReceiveTimeout = 10000
            };

            try
            {
                await server.PerformQueryAsync();
                var information = await server.GetInformationAsync();
                var game = _context.Games.FirstOrDefault(x => x.GameSteamId == information.GameId);

                if (information.OnlinePlayers == null || information.MaxPlayers == null)
                {
                    ModelState.AddModelError(string.Empty, "Не удалось получить информацию о игроках.");
                    return Page();
                }

                ServerEntity.CreateTime = DateTime.Now.ToString("f");
                ServerEntity.ServerMap = information.Map;
                ServerEntity.ServerVersion = information.Version;
                ServerEntity.ServerName = information.ServerName;
                ServerEntity.ServerOnline = $"{information.OnlinePlayers} / {information.MaxPlayers}";
                ServerEntity.Location = await GetCountryByIp.GetCountryAsync(ServerEntity.ServerIP);
                ServerEntity.GamePort = information.Port.ToString();
                ServerEntity.Game = game;
                ServerEntity.IsOnline = true;

                var onlineLog = new OnlineLog
                {
                    OnlinePlayers = $"{information.OnlinePlayers} / {information.MaxPlayers}",
                    Date = DateTime.Now
                };
                ServerEntity.OnlineLog = new List<OnlineLog> { onlineLog };

                server.Close();
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, $"Произошла ошибка: {ex.Message}");
                return Page(); 
            }

            await _context.Servers.AddAsync(ServerEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ConfirmServer", new { id = ServerEntity.Id });
        }
    }
}
