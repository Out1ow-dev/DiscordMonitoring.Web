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
    public class DeleteModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public DeleteModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GameEntity GameEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameentity = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);

            if (gameentity == null)
            {
                return NotFound();
            }
            else
            {
                GameEntity = gameentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameentity = await _context.Games.FindAsync(id);
            if (gameentity != null)
            {
                GameEntity = gameentity;
                _context.Games.Remove(GameEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./GamesList");
        }
    }
}
