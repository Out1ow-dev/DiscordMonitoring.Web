using DiscordMonitoring.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiscordMonitoring.Web.Pages
{
    public class ServersModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public ServersModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public GameEntity Game { get; set; }
        public List<ServerEntity> ServerEntityList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);

            var servers = await _context.Servers
                .Include(m => m.Game) 
                .Where(m => m.Game != null && m.Game.Id == id) 
                .ToListAsync();

            if (servers == null || !servers.Any()) 
            {
                return NotFound();
            }

            ServerEntityList = servers;

            return Page();
        }

    }
}
