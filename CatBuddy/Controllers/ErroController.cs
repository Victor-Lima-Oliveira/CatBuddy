using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ErroController : Controller
    {
        public IActionResult MostrarErro(int codErro)
        {
            if (TempData[Const.ErroTempData] != null)
            {
                ViewBag.ERRO = TempData[Const.ErroTempData];
            }
            if (Const.ErroProdutoNaoEncontrado == 1)
            {
                ViewBag.ERRO = Strings.ProdutoNaoEncontrado;
            }
            return View();
        }
    }
}
