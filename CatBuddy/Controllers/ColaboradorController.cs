using CatBuddy.LibrariesSessao.Filtro;
using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using CatBuddy.Repository;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;
        private LoginColaborador _loginColaborador;

        public ColaboradorController(IColaboradorRepository colaboradorRepository, LoginColaborador loginColaborador)
        {
            _colaboradorRepository = colaboradorRepository;
            _loginColaborador = loginColaborador;
        }

        /// <summary>
        /// Login do funcionario
        /// </summary>
        public IActionResult Index()
        {
            // TODO: Se o usuario não estiver cadastrado, abre o index
            // Se estiver cadastrado, manda para a paginaPrincipal

            return RedirectToAction(nameof(PainelColaborador), "Colaborador");
        }

        [ColaboradorAutorizacao]
        public IActionResult PainelColaborador()
        {
            if (TempData[Const.AvisoPaginaPrincipalSucesso] != null)
            {
                ViewBag.AvisoPaginaPrincipal = TempData[Const.AvisoPaginaPrincipalSucesso];
            }
            return View(_loginColaborador.GetColaborador());
        }

        [ColaboradorAutorizacao]
        public IActionResult CadastrarColaborador()
        {
            return View();
        }
        public IActionResult Sair()
        {
            // Remove o colaborador da sessão
            _loginColaborador.Logout();

            // Vai para tela de login
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Colaborador colaborador)
        {
            Colaborador colaboradorDoBanco = _colaboradorRepository.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDoBanco.Email != null && colaboradorDoBanco.Senha != null)
            {
                _loginColaborador.Login(colaboradorDoBanco);
                return new RedirectResult(Url.Action(nameof(PainelColaborador)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não localizado, verifique se o e-mail e senha estão digitados corretamente";
                return View();
            }
        }
    }
}
