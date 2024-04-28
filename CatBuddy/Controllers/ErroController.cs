using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ErroController : Controller
    {
        public IActionResult MostrarErro()
        {
            if (TempData[Const.ErroTempData] != null)
            {
                ViewBag.ERRO = TempData[Const.ErroTempData];
            } 
            return View();
        }
    }
}
