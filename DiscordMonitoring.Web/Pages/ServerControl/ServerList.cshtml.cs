using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;
using System.Security.Claims;

namespace DiscordMonitoring.Web.Pages.ServerControl
{
    public class ServerListModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public ServerListModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ServerEntity> ServerEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ServerEntity = await _context.Servers.Where(s => s.User.Id == userId).ToListAsync();
        }

    }
}
