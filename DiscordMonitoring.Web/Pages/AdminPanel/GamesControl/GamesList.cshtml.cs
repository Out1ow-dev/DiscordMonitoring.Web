using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;

namespace DiscordMonitoring.Web.Pages.AdminPanel.GamesControl
{
    public class GamesListModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public GamesListModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GameEntity> GameEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            GameEntity = await _context.Games.ToListAsync();
        }
    }
}
