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

namespace DiscordMonitoring.Web.Pages.AdminPanel.GamesControl
{
    public class EditModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public EditModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
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

            var gameentity =  await _context.Games.FirstOrDefaultAsync(m => m.Id == id);
            if (gameentity == null)
            {
                return NotFound();
            }
            GameEntity = gameentity;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(GameEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameEntityExists(GameEntity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./GamesList");
        }

        private bool GameEntityExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
