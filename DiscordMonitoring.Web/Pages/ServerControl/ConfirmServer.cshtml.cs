using DiscordMonitoring.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SteamQuery;
using System.Security.Claims;

namespace DiscordMonitoring.Web.Pages.ServerControl
{
    [Authorize]
    public class ConfirmServerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ConfirmServerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ServerId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ServerId = id;
            var server = _context.Servers.FirstOrDefault(s => s.Id == id);
            if (server != null)
            {
                var confirmString = GenerateRandomString();
                ViewData["ConfirmString"] = confirmString;
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        private string GenerateRandomString()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var length = 10;
            var result = "";
            for (var i = 0; i < length; i++)
            {
                result += characters[new Random().Next(0, characters.Length)];
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(int id, string confirmString)
        {
            var server = _context.Servers.FirstOrDefault(s => s.Id == id);
            if (server != null)
            {
                var gameServer = new GameServer($"{server.ServerIP}:{server.QueryPort}")
                {
                    SendTimeout = 10000,
                    ReceiveTimeout = 10000
                };
                try
                {
                    await gameServer.PerformQueryAsync();
                    var information = await gameServer.GetInformationAsync();
                    if (information.ServerName.Contains(confirmString))
                    {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var userManager = (UserManager<IdentityUser>)HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>));
                        var user = await userManager.FindByIdAsync(userId);
                        server.User = user;

                        _context.Servers.Update(server);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("./ServerControl/ServerList");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Фраза не найдена!");
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка!");
                    return Page();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }

}
