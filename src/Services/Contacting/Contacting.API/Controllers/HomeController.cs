using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.Contacting.API.Controllers
{
    public class HomeController : ControllerBase
    {
        // GET: /<controller>/

        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
