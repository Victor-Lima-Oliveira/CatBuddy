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

        ClienteController(IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDoBanco = _clienteRepository.Login(cliente.Email, cliente.Senha);
            
            if(clienteDoBanco.Email != null && clienteDoBanco.Senha != null)
            {
                _loginCliente.Login(clienteDoBanco);
            }
            return View(cliente);
        }
    }
}
