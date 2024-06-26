using CatBuddy.httpContext;
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
        private IPedidoRepository _pedidoRepository;
        private IEnderecoRepository _enderecoRepository;
        private ICartaoRepository _cartaoRepository;
        private CarrinhoDeCompraCookie _carrinhoDeCompraCookie;
        private LoginCliente _loginCliente;

        public ClienteController(IClienteRepository clienteRepository, LoginCliente loginCliente, IUsuarioRepository usuarioRepository, IPedidoRepository pedidoRepository, IEnderecoRepository enderecoRepository, ICartaoRepository cartaoRepository)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _usuarioRepository = usuarioRepository;
            _pedidoRepository = pedidoRepository;
            _enderecoRepository = enderecoRepository;
            _cartaoRepository = cartaoRepository;
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

                MainLayout.codCliente = cliente.cod_id_cliente.Value;

                // Volta para a página principal para fazer as comprar 
                return RedirectToAction("Index", "Home");
            }

            // Se aconteceu algum erro retorna para a mesma página
            return View(CarregaViewCliente(cliente));
        }

        public IActionResult Sair()
        {
            _loginCliente.Logout();

            MainLayout.codCliente = 0;
            MainLayout.EnderecoSelecionado = null;
            MainLayout.qtdCarrinho = 0;

            return RedirectToAction("index", "Home");
        }

        public IActionResult Login()
        {
            if (_loginCliente.ObterCliente() != null)
            {
                return RedirectToAction(nameof(PainelCliente));
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDoBanco = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if (clienteDoBanco.Email != null && clienteDoBanco.Senha != null)
            {
                _loginCliente.Login(clienteDoBanco);

                MainLayout.codCliente = clienteDoBanco.cod_id_cliente.Value;

                return RedirectToAction("AtualizaQtd", "Carrinho");
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

        [ClienteAutorizacao]
        public IActionResult VisualizarPedidos()
        {
            return View(_pedidoRepository.ObtemPedidos(_loginCliente.ObterCliente().cod_id_cliente.Value));
        }

        [ClienteAutorizacao]
        public IActionResult DetalharPedido(int codPedido)
        {
            return View(_pedidoRepository.ObtemItensPedido(codPedido));
        }

        [ClienteAutorizacao]
        public IActionResult CadastrarEndereco()
        {
            return View(CarregaViewEndereco());
        }

        [ClienteAutorizacao]
        [HttpPost]
        public IActionResult CadastrarEndereco(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                // Recupera o código do cliente
                endereco.cod_cliente = MainLayout.codCliente;
                endereco.cepUsuario = Apoio.TransformaCEP(endereco.cepUsuario);

                // Cadastra o endereco no banco
                _enderecoRepository.Cadastrar(endereco);

                MainLayout.OpenSnackbar("Endereço cadastrado com sucesso!");

                return RedirectToAction(nameof(PainelCliente));
            }

            return View(CarregaViewEndereco(endereco));
        }

        [ClienteAutorizacao]
        public IActionResult VisualizarEnderecos(bool bSelecionar = false)
        {
            if (bSelecionar)
            {
                ViewBag.selecionar = true;
            }
            else
            {
                ViewBag.selecionar = false;
            }

            return View(_enderecoRepository.ObtemEnderecos(MainLayout.codCliente));
        }

        [HttpGet]
        public IActionResult EditarEndereco(int id)
        {
            Endereco endereco = _enderecoRepository.ObtemEndereco(id);
            return View(CarregaViewEndereco(endereco));
        }

        [ClienteAutorizacao]
        [HttpPost]
        public IActionResult EditarEndereco(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                endereco.cepUsuario = Apoio.TransformaCEP(endereco.cepUsuario);
                endereco.cod_cliente = MainLayout.codCliente;

                // Atualiza o endereco
                _enderecoRepository.Atualizar(endereco);

                MainLayout.OpenSnackbar("Endereço atualizado com sucesso!");

                return RedirectToAction(nameof(PainelCliente));
            }
            return View();
        }

        public IActionResult DeletarEndereco(int id, string nome)
        {
            MainLayout.OpenDialog(nome, Strings.ConfirmaDeletarEndereco, id);
            return RedirectToAction(nameof(VisualizarEnderecos));
        }

        public IActionResult ExecutarDialog()
        {
            try
            {
                if (MainLayout.ConteudoDialog == Strings.ConfirmaDeletarEndereco)
                {
                    // Exclui o endereco no banco
                    _enderecoRepository.Excluir(Convert.ToInt32(MainLayout.ObterParametro()));

                    MainLayout.OpenSnackbar("Endereço deletado com sucesso!");

                    MainLayout.CloseDialog();

                    return RedirectToAction(nameof(VisualizarEnderecos));

                }
                else if (MainLayout.ConteudoDialog == Strings.ConfirmaDeletarCartao)
                {
                    _cartaoRepository.Excluir(Convert.ToInt32(MainLayout.ObterParametro()));

                    MainLayout.CloseDialog();

                    MainLayout.OpenSnackbar("Cartão deletado com sucesso!");

                    return RedirectToAction(nameof(VisualizarCartoes));
                }
            }
            catch (Exception err)
            {
                MainLayout.OpenDialog("ERRO", err.Message);
                return RedirectToAction(nameof(VisualizarEnderecos));
            }

            return RedirectToAction(nameof(PainelCliente));
        }

        public ViewEndereco CarregaViewEndereco(Endereco endereco = null)
        {
            ViewEndereco viewEndereco = new ViewEndereco()
            {
                ListLogradouro = _usuarioRepository.RetornaLogradouro(),
            };

            if (endereco != null)
            {
                viewEndereco.Endereco = endereco;
            }
            else
            {
                //viewEndereco.Endereco.cod_cliente = _loginCliente
            }

            return viewEndereco;
        }

        [ClienteAutorizacao]
        public IActionResult VisualizarCartoes()
        {
            return View(_cartaoRepository.ObtemCartoes(MainLayout.codCliente));
        }

        [ClienteAutorizacao]
        public IActionResult CadastrarCartao()
        {
            return View();
        }

        [ClienteAutorizacao]
        [HttpPost]
        public IActionResult CadastrarCartao(Cartao cartao)
        {
            if (ModelState.IsValid)
            {
                cartao.numeroCartaoCred = Apoio.TransformaNumeroCartaoCredito(cartao.numeroCartaoCred);
                cartao.dataDeValidade = Apoio.TransformaDataValidade(cartao.dataDeValidade);

                _cartaoRepository.Cadastrar(cartao);

                MainLayout.OpenSnackbar("Cartão cadastrado!");

                return RedirectToAction(nameof(VisualizarCartoes));
            }

            return View(cartao);
        }

        [ClienteAutorizacao]
        public IActionResult EditarCartao(int id)
        {
            return View(_cartaoRepository.ObtemCartao(id));
        }

        [ClienteAutorizacao]
        [HttpPost]
        public IActionResult EditarCartao(Cartao cartao)
        {
            if (ModelState.IsValid)
            {
                cartao.numeroCartaoCred = Apoio.TransformaNumeroCartaoCredito(cartao.numeroCartaoCred);
                cartao.dataDeValidade = Apoio.TransformaDataValidade(cartao.dataDeValidade);

                _cartaoRepository.Atualizar(cartao);

                return RedirectToAction(nameof(VisualizarCartoes));
            }
            return View(cartao);
        }

        public IActionResult DeletarCartao(int id, string titulo)
        {
            MainLayout.OpenDialog(titulo, Strings.ConfirmaDeletarCartao, id);
            return RedirectToAction(nameof(VisualizarCartoes));
        }

        public IActionResult SelecionarEndereco(int id)
        {
            Endereco endereco = _enderecoRepository.ObtemEndereco(id);

            MainLayout.EnderecoSelecionado = endereco; 

            MainLayout.OpenSnackbar($"Endereço selecionado: {MainLayout.EnderecoSelecionado.nomeEnderecoUsuario}");

            return RedirectToAction("Pagamento", "Carrinho");
        }
    }
}
