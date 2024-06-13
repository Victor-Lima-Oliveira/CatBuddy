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
        private IUsuarioRepository _usuarioRepository;
        private LoginColaborador _loginColaborador;

        public ColaboradorController(IColaboradorRepository colaboradorRepository, LoginColaborador loginColaborador, IUsuarioRepository usuarioRepository)
        {
            _colaboradorRepository = colaboradorRepository;
            _loginColaborador = loginColaborador;
            _usuarioRepository = usuarioRepository;
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

        public IActionResult VisualizarColaboradores()
        {
            return View(_colaboradorRepository.ObterTodosColaboradores());
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
            return View(CarregaViewColaborador());
        }

        [HttpPost]
        [ColaboradorAutorizacao]
        public IActionResult CadastrarColaborador(Colaborador colaborador)
        {
            // Se passou na validação
            if (ModelState.IsValid)
            {
                // Se for selecionado um genero e um nivel de acesso, insere o colaborador no banco 
                if(colaborador.codGenero != Const.SEM_GENERO && colaborador.NivelDeAcesso != Const.SEM_NIVEL_ACESSO)
                {
                    return RedirectToAction(nameof(VisualizarColaboradores));
                }
            }
            
            // Se não passou na validação, apresenta a mesma página 
            return View(CarregaViewColaborador());
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

        public ViewColaborador CarregaViewColaborador()
        {
            ViewColaborador viewColaborador = new ViewColaborador()
            {
                Colaborador = new Colaborador(),
                ListGenero = _usuarioRepository.RetornaGenero(),
                ListNivelAcesso = _colaboradorRepository.ObtemNivelDeAcesso(),
            };

            return viewColaborador; 
        }
    }
}
