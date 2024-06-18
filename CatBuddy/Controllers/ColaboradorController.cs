using CatBuddy.LibrariesSessao.Filtro;
using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using CatBuddy.Repository;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [ColaboradorAutorizacao]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(PainelColaborador), "Colaborador");
        }

        [ColaboradorAutorizacao]
        public IActionResult AbrirDialogDesativaColaborador(int id, string nomeColaborador)
        {
            MainLayout.OpenDialog(nomeColaborador, "Deseja DELETAR esse colaborador?", id);
            return RedirectToAction(nameof(VisualizarColaboradores));
        }

        [ColaboradorAutorizacao]
        public IActionResult ExecutarDialog()
        {
            try
            {
                // Desativa o colaborador no banco
                _colaboradorRepository.Excluir(Convert.ToInt32(MainLayout.ObterParametro()));

                MainLayout.CloseDialog();
            }
            catch (Exception err)
            {
                MainLayout.OpenDialog("ERRO", err.Message);
                return RedirectToAction(nameof(VisualizarColaboradores));
            }

            return RedirectToAction(nameof(VisualizarColaboradores));
        }

        [ColaboradorAutorizacao]
        public IActionResult FecharDialog()
        {
            TempData["OpenDialog"] = null;
            TempData["nomeColaborador"] = null;
            TempData["idColaborador"] = null;

            return RedirectToAction(nameof(VisualizarColaboradores));
        }

        [ColaboradorAutorizacao]
        public IActionResult VisualizarColaboradores()
        {
            if (TempData["OpenDialog"] != null)
            {
                ViewBag.OpenDialog = "true";
            }
            if (TempData["nomeColaborador"] != null)
            {
                ViewBag.nomeColaborador = TempData["nomeColaborador"];
            }

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
        public IActionResult EditarColaborador(int id)
        {
            // Recupera o colaborador
            Colaborador colaboradorAux = _colaboradorRepository.ObterColaborador(id);

            // Abre a página
            return View(CarregaViewColaborador(colaboradorAux));
        }

        [HttpPost]
        [ColaboradorAutorizacao]
        public IActionResult EditarColaborador(Colaborador colaborador)
        {
            // Se passar na validação
            if (ModelState.IsValid && ValidaCampos(colaborador))
            {
                // Remove os caracteres extras
                colaborador.CPF = Apoio.TransformaCPF(colaborador.CPF);
                colaborador.Telefone = Apoio.TransformaTelefone(colaborador.Telefone);

                // Atualiza o colaborador
                _colaboradorRepository.Atualizar(colaborador);

                // Volta para a página do layout
                return RedirectToAction(nameof(VisualizarColaboradores));
            }

            // Retorna para a mesma view caso erro 
            return View(CarregaViewColaborador(colaborador));
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
            if (ModelState.IsValid && ValidaCampos(colaborador))
            {
                // Remove os caracteres extras dos campos
                colaborador.CPF = Apoio.TransformaCPF(colaborador.CPF);
                colaborador.Telefone = Apoio.TransformaTelefone(colaborador.Telefone);

                // Cadastra o usuario no banco 
                _colaboradorRepository.Cadastrar(colaborador);

                // Recupera os dados do colaborador 
                colaborador = _colaboradorRepository.Login(colaborador.Email, colaborador.Senha);

                // Salva os dados do colaborador na sessão 
                _loginColaborador.Login(colaborador);

                // Redireciona para a lista de colaboradores
                return RedirectToAction(nameof(VisualizarColaboradores));
            }

            // Se não passou na validação, apresenta a mesma página 
            return View(CarregaViewColaborador(colaborador));
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

        public ViewColaborador CarregaViewColaborador(Colaborador colaborador = null)
        {
            ViewColaborador viewColaborador = new ViewColaborador();
            viewColaborador.ListGenero = _usuarioRepository.RetornaGenero();
            viewColaborador.ListNivelAcesso = _colaboradorRepository.ObtemNivelDeAcesso();

            if (colaborador == null)
            {
                viewColaborador.Colaborador = new Colaborador();
            }
            else
            {
                viewColaborador.Colaborador = colaborador;
            }

            return viewColaborador;
        }

        public bool ValidaCampos(Colaborador colaborador)
        {
            bool bValido = true;

            // Se o usuário selecionou um genero
            if (colaborador.codGenero == Const.SEM_GENERO)
            {
                bValido = false;
                ModelState.AddModelError("Colaborador.codGenero", "Selecione um gênero!");
            }

            // Se o usuario selecionou um nível de acesso
            if (colaborador.NivelDeAcesso == Const.SEM_NIVEL_ACESSO)
            {
                bValido = false;
                ModelState.AddModelError("Colaborador.NivelDeAcesso", "Selecione o nível de acesso!");
            }

            return bValido;
        }
    }
}
