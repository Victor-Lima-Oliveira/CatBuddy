using CatBuddy.LibrariesSessao.Filtro;
using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;

        public ClienteController(IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ClienteAutorizacao]
        public IActionResult PainelCliente()
        {
            Cliente cliente = _loginCliente.ObterCliente();
            return View(cliente);
        }

        public IActionResult Sair()
        {
            _loginCliente.Logout();

            return RedirectToAction("index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDoBanco = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if (clienteDoBanco.Email != null && clienteDoBanco.Senha != null)
            {
                _loginCliente.Login(clienteDoBanco);
                return new RedirectResult(Url.Action(nameof(PainelCliente)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não localizado, verifique se o e-mail e senha estão digitados corretamente";
                return View();
            }
        }
    }
}
