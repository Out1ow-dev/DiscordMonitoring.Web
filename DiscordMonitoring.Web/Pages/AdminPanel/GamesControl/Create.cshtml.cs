using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;

namespace DiscordMonitoring.Web.Pages.AdminPanel.GamesControl
{
    public class CreateModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public CreateModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public GameEntity GameEntity { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Games.Add(GameEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./GamesList");
        }
    }
}
