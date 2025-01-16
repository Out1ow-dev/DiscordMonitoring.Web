using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DiscordMonitoring.Web.Data;
using DiscordMonitoring.Web.Model;

namespace DiscordMonitoring.Web.Pages.ServerControl
{
    public class DeleteModel : PageModel
    {
        private readonly DiscordMonitoring.Web.Data.ApplicationDbContext _context;

        public DeleteModel(DiscordMonitoring.Web.Data.ApplicationDbContext context)
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

            var serverentity = await _context.Servers.FirstOrDefaultAsync(m => m.Id == id);

            if (serverentity == null)
            {
                return NotFound();
            }
            else
            {
                ServerEntity = serverentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serverentity = await _context.Servers.FindAsync(id);
            if (serverentity != null)
            {
                ServerEntity = serverentity;
                _context.Servers.Remove(ServerEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ServerList");
        }
    }
}
