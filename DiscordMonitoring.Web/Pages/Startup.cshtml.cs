using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscordMonitoring.Web.Pages
{
    public class StartupModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public StartupModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
