using DiscordMonitoring.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiscordMonitoring.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public IndexModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GameEntity> GamesEntity { get; set; } = default!;

        public async Task OnGetAsync()
        {
            GamesEntity = await _context.Games.ToListAsync();
        }
    }
}
