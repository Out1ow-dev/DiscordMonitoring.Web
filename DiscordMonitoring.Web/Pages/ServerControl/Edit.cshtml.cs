using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DiscordMonitoring.Web.Pages.ServerControl
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public EditModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ServerEntity ServerEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serverentity = await _context.Servers
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serverentity == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (serverentity.User == null || serverentity.User.Id != userId)
            {
                return Forbid();
            }

            ServerEntity = serverentity;

            ViewData["GameId"] = new SelectList(_context.Games, "Id", "GameName");
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("ServerEntity.ServerName");
            ModelState.Remove("ServerEntity.ServerOnline");
            ModelState.Remove("ServerEntity.ServerVersion");
            ModelState.Remove("ServerEntity.ServerMap");
            ModelState.Remove("ServerEntity.Location");
            ModelState.Remove("ServerEntity.CreateTime");
            ModelState.Remove("ServerEntity.GameId");
            ModelState.Remove("ServerEntity.GamePort");


            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var server = _context.Servers
                .Include(s => s.User)
                .FirstOrDefault(m => m.Id == ServerEntity.Id);

            if (server == null || server.User == null || server.User.Id != userId)
            {
                return Forbid();
            }

            if (server != null)
            {
                server.ServerDescription = ServerEntity.ServerDescription;
                server.QueryPort = ServerEntity.QueryPort;
                server.ServerIP = ServerEntity.ServerIP;
            }
            else
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerEntityExists(ServerEntity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ServerList");
        }


        private bool ServerEntityExists(int id)
        {
            return _context.Servers.Any(e => e.Id == id);
        }
    }
}
