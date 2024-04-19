using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
