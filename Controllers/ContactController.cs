using Microsoft.AspNetCore.Mvc;

namespace PortfolioProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
