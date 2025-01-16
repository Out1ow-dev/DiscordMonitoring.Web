using DiscordMonitoring.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiscordMonitoring.Web.Pages
{
    public class ServerModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public ServerModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ServerEntity Server { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .Include(m => m.Game)
                .Include(u => u.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (server == null)
            {
                return NotFound();
            }

            Server = server;

            var onlineLogs = await _context.onlineLogs
             .Where(x => x.ServerId == id)
             .OrderBy(x => x.Date)
             .ToListAsync();

            var groupedLogs = onlineLogs
                .Select((x, i) => new { x, i })
                .GroupBy(x => x.i / 5)
                .Select(g => new
                {
                    Date = g.First().x.Date,
                    AverageOnline = g.Select(x => int.Parse(x.x.OnlinePlayers.Split('/')[0].Trim())).Average(),
                    MaxOnline = g.Select(x => int.Parse(x.x.OnlinePlayers.Split('/')[0].Trim())).Max()
                })
                .ToArray();

            ViewData["OnlineLogDates"] = groupedLogs.Select(x => x.Date.ToString("yyyy-MM-dd HH:mm")).ToArray();
            ViewData["OnlineLogAverageOnline"] = groupedLogs.Select(x => x.AverageOnline).ToArray();
            ViewData["OnlineLogMaxOnline"] = groupedLogs.Select(x => x.MaxOnline).ToArray();
            return Page();
        }

    }
}
