using CatBuddy.LibrariesSessao.Filtro;
using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using CatBuddy.Repository;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;

namespace CatBuddy.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IUsuarioRepository _usuarioRepository;
        private LoginCliente _loginCliente;

        public ClienteController(IClienteRepository clienteRepository, LoginCliente loginCliente, IUsuarioRepository usuarioRepository)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _usuarioRepository = usuarioRepository;
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

        public IActionResult Cadastrar()
        {
            return View(CarregaViewCliente());
        }

        [HttpPost]
        public IActionResult Cadastrar(Cliente cliente)
        {
            bool bValido = true;

            // Se não selecionar um genero
            if (cliente.codGenero == Const.SEM_GENERO)
            {
                ModelState.AddModelError("Cliente.codGenero", "Selecione um gênero");
                bValido = false;
            }

            // Se for menor de idade
            if (cliente.DtNascimento > DateTime.Now.AddYears(-18))
            {
                ModelState.AddModelError("Cliente.DtNascimento", "É preciso ser maior de idade para criar uma conta");
                bValido = false;
            }

            // Se passar em todas as validações 
            if (ModelState.IsValid && bValido)
            {
                // Remove os caracteres do CPF e telefone
                cliente.CPF = Apoio.TransformaCPF(cliente.CPF);
                cliente.Telefone = Apoio.TransformaTelefone(cliente.Telefone);
                
                // Cadastra o cliente no banco
                _clienteRepository.Cadastrar(cliente);

                //Recupera os dados completos do cliente
                cliente = _clienteRepository.Login(cliente.Email, cliente.Senha);

                // Salva ele na sessão
                _loginCliente.Login(cliente);

                // Volta para a página principal para fazer as comprar 
                return RedirectToAction("Index", "Home");
            }

            // Se aconteceu algum erro retorna para a mesma página
            return View(    CarregaViewCliente(cliente));
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
        public ViewCliente CarregaViewCliente(Cliente clienteRecebido = null)
        {
            ViewCliente viewColaborador = new ViewCliente();

            // Recupera a lista com generos
            viewColaborador.ListGenero = _usuarioRepository.RetornaGenero();

            // Se não recebeu um cliente
            if (clienteRecebido == null)
            {
                viewColaborador.cliente = new Cliente();
            }
            // Se ja tinha um cliente na model, persiste
            else
            {
                viewColaborador.cliente = clienteRecebido;
            }

            return viewColaborador;
        }
    }
}
