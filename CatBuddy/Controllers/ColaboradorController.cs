using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ColaboradorController : Controller
    {
        /// <summary>
        /// Login do funcionario
        /// </summary>
        public IActionResult Index()
        {
            // TODO: Se o usuario não estiver cadastrado, abre o index
            // Se estiver cadastrado, manda para a paginaPrincipal

            return RedirectToAction("PaginaPrincipal", "Colaborador");
        }

        public IActionResult PaginaPrincipal()
        {
            if (TempData[Const.AvisoPaginaPrincipalSucesso] != null)
            {
                ViewBag.AvisoPaginaPrincipal = TempData[Const.AvisoPaginaPrincipalSucesso];
            }
            return View();
        }
        public IActionResult CadastrarColaborador()
        {
            return View();
        }
        public IActionResult Sair()
        {
            // TODO: remover o usuario da sessão 
            return RedirectToAction("Index", "Home");
        }
    }
}
